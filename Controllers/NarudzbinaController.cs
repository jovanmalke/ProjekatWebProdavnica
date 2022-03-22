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
    public class NarudzbinaController : ControllerBase
    {
        public OnlineProdavnicaContext Context { get; set; }

        public NarudzbinaController(OnlineProdavnicaContext context)
        {
            Context = context;
        }

        [EnableCors("CORS")]
        [Route("PreuzmiNarudzbu/{adresa}/{brTelefona}")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiNarudzbu(string adresa, string brTelefona)
        {
            if (string.IsNullOrWhiteSpace(adresa) || adresa.Length > 50)
            {
                return BadRequest("Nevalidna adresa");
            }
            if (string.IsNullOrWhiteSpace(brTelefona))
            {
                return BadRequest("Nevalidan brtelefona");
            }
            try
            {
                var narudzbina = await Context.Narudzbine.Where(p => p.Adresa == adresa && p.BrTelefona == brTelefona)
                .Include(p => p.NarudzbinaProdavnica)
                .Select(p => new
                {
                    p.ID,
                    IDProdavnice = p.NarudzbinaProdavnica.ID,
                    p.Adresa,
                    p.BrTelefona
                }).FirstOrDefaultAsync();
                if (narudzbina != null)
                {
                    return Ok(narudzbina);
                }
                else
                {
                    return BadRequest("Nije nadjena narudzba");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("PreuzmiNarudzbu")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiNarudzbu()
        {
            var narudzbine = await Context.Narudzbine
            .Include(p => p.NarudzbinaProdavnica)
            .Select(p => new
            {
                p.ID,
                IDProdavnice = p.NarudzbinaProdavnica.ID
            }).ToListAsync();
            return Ok(narudzbine);
        }

        [EnableCors("CORS")]
        [Route("DodajNarudzbinu")]
        [HttpPost]
        public async Task<ActionResult> DodajNarudzbinu([FromBody] Narudzbina narudzbina)
        {
            if (string.IsNullOrWhiteSpace(narudzbina.Adresa) || narudzbina.Adresa.Length > 50)
            {
                return BadRequest("Nevalidna adresa");
            }
            if (string.IsNullOrWhiteSpace(narudzbina.BrTelefona))
            {
                return BadRequest("Nevalidan brtelefona");
            }
            try
            {
                Context.Narudzbine.Add(narudzbina);
                await Context.SaveChangesAsync();
                return Ok("Uspesno dodata narudzbina");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("DodajNarudzbinu/{adresa}/{brTelefona}/{nazivProd}/{idD}")]
        [HttpPost]
        public async Task<ActionResult> DodajNarudzbinu(string adresa, string brTelefona, string nazivProd, int idD)
        {
            if (string.IsNullOrWhiteSpace(adresa) || adresa.Length > 50)
            {
                return BadRequest("Nevalidna adresa");
            }
            if (string.IsNullOrWhiteSpace(brTelefona))
            {
                return BadRequest("Nevalidan brtelefona");
            }
            if (string.IsNullOrWhiteSpace(nazivProd) || nazivProd.Length > 20)
            {
                return BadRequest("Nevalidno ime prodavnice");
            }
            if (idD < 0)
            {
                return BadRequest("Nevalidan ID");
            }
            try
            {
                var prodavnica = Context.Prodavnice.Where(p => p.Naziv == nazivProd).FirstOrDefault();
                var dostavljac = Context.Dostavljaci.Where(p => p.ID == idD).FirstOrDefault();
                var nar = Context.Narudzbine.Where(p => p.Adresa == adresa && p.BrTelefona == brTelefona).FirstOrDefault();
                if (nar == null)
                {
                    if (prodavnica != null && dostavljac != null)
                    {
                        Narudzbina d = new Narudzbina
                        {
                            Adresa = adresa,
                            BrTelefona = brTelefona,
                            NarudzbinaProdavnica = prodavnica,
                            NarudzbinaDostavljac = dostavljac
                        };
                        Context.Narudzbine.Add(d);
                        await Context.SaveChangesAsync();
                        return Ok("Uspesno dodata narudzbina");
                    }
                    else
                    {
                        return BadRequest("Nije prodnadjena prodavnica ili dostavljac");
                    }
                }
                else
                {
                    return BadRequest("Narudzbina vec postoji");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzbrisiNarudzbinu/{id}")]
        [HttpDelete]

        public async Task<ActionResult> IzbrisiNarudzbinu(int id)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan id");
            }
            try
            {
                var narudzbina = Context.Narudzbine.Where(p => p.ID == id).FirstOrDefault();
                if (narudzbina != null)
                {
                    Context.Narudzbine.Remove(narudzbina);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izbirisana narudzbina");
                }
                else
                {
                    return BadRequest("Narudzbina nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzmeniNarudzbinu/{id}/{adresa}/{brtelefona}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniNarudzbinuBody(int id, string adresa, string brtelefona)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan id");
            }
            if (string.IsNullOrWhiteSpace(adresa) || adresa.Length > 50)
            {
                return BadRequest("Nevalidna adresa");
            }
            if (string.IsNullOrWhiteSpace(brtelefona))
            {
                return BadRequest("Nevalidan brtelefona");
            }
            try
            {
                var narudzbinaZaPromenu = Context.Narudzbine.Where(p => p.ID == id).FirstOrDefault();
                if (narudzbinaZaPromenu != null)
                {
                    narudzbinaZaPromenu.BrTelefona = brtelefona;
                    narudzbinaZaPromenu.Adresa = adresa;
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izmenanjena narudzbina");
                }
                else
                {
                    return BadRequest("Narudzbina nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("DodajProizvodUNarudzbu/{idN}/{idS}")]
        [HttpPut]

        public async Task<ActionResult> DodajProizvodUNarudzbu(int idN, int idS)
        {
            if (idN < 0 || idS < 0)
            {
                return BadRequest("Nevalidan id");
            }
            try
            {
                var narudzbina = Context.Narudzbine.Where(p => p.ID == idN).FirstOrDefault();
                var spoj = Context.ProizvodNarudzbina.Where(p => p.ID == idS).FirstOrDefault();
                if (narudzbina != null && spoj != null)
                {
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno dodat proizvod u narudzbinu");
                }
                else
                {
                    return BadRequest("Nisu nadjeni");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
