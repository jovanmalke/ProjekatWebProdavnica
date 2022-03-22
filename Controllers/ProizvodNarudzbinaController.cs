using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace PrWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProizvodNarudzbinaController : ControllerBase
    {
        public OnlineProdavnicaContext Context { get; set; }

        public ProizvodNarudzbinaController(OnlineProdavnicaContext context)
        {
            Context = context;
        }

        [EnableCors("CORS")]
        [Route("DodajProizvodUNarudzbinu/{adresaNarudzbine}/{brtelefona}/{imeProizvoda}/{kolicinaUNarudzbini}")]
        [HttpPost]
        public async Task<ActionResult> DodajProizvodUProdavnicu(string adresaNarudzbine, string brtelefona, string imeProizvoda, int kolicinaUNarudzbini)
        {
            if (string.IsNullOrWhiteSpace(adresaNarudzbine) || adresaNarudzbine.Length > 50)
            {
                return BadRequest("Nevalidna adresa narudzbine");
            }
            if (string.IsNullOrWhiteSpace(brtelefona))
            {
                return BadRequest("Nevalidan broj telefona");
            }
            if (string.IsNullOrWhiteSpace(imeProizvoda) || imeProizvoda.Length > 20)
            {
                return BadRequest("Nevalidno ime proizvoda");
            }
            try
            {
                var narudzba = Context.Narudzbine.Where(p => p.Adresa == adresaNarudzbine && p.BrTelefona == brtelefona).FirstOrDefault();
                if (narudzba == null)
                {
                    return BadRequest("Nije pronadjena narudzbina");
                }
                var narudzbina = Context.Narudzbine.Where(p => p.Adresa == adresaNarudzbine && p.BrTelefona == brtelefona)
                .Include(p => p.NarudzbinaProdavnica)
                .Select(p => new
                {
                    p.ID,
                    p.Adresa,
                    p.BrTelefona,
                    IDProdavnice = p.NarudzbinaProdavnica.ID
                }).FirstOrDefault();
                var proizvod = Context.Proizvodi.Where(p => p.Ime == imeProizvoda).FirstOrDefault();
                if (proizvod == null)
                {
                    return BadRequest("Nije pronadjen proizvod");
                }
                var spoj = Context.ProdavnicaProizvod
                .Include(p => p.ProdavnicaSpoj1)
                .Include(p => p.ProizvodSpoj1)
                .Where(p => p.ProdavnicaSpoj1.ID == narudzbina.IDProdavnice && p.ProizvodSpoj1.ID == proizvod.ID)
                .Select(p => new
                {
                    p.Cena
                })
                .FirstOrDefault();
                if (narudzba != null && proizvod != null)
                {
                    ProizvodNarudzbinaSpoj ovoDodaj = new ProizvodNarudzbinaSpoj
                    {
                        KolicinaProizvodaUNarudzbini = kolicinaUNarudzbini,
                        NarudzbinaSpoj2 = narudzba,
                        ProizvodSpoj2 = proizvod,
                        Cena = spoj.Cena * kolicinaUNarudzbini
                    };
                    Context.ProizvodNarudzbina.Add(ovoDodaj);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno dodat");
                }
                else
                {
                    return BadRequest("Ne postoji narudzbina ili proizvod");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
