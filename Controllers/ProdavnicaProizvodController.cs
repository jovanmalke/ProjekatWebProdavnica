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
    public class ProdavnicaProizvodController : ControllerBase
    {
        public OnlineProdavnicaContext Context { get; set; }

        public ProdavnicaProizvodController(OnlineProdavnicaContext context)
        {
            Context = context;
        }

        [EnableCors("CORS")]
        [Route("PreuzmiImenaProizvoda")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiImenaProizvoda()
        {
            var proizvodiProdavnice = await Context.ProdavnicaProizvod
            .Where(p => p.Kolicina > 0)
            .Include(p => p.ProdavnicaSpoj1)
            .Include(p => p.ProizvodSpoj1)
            .Select(p => new
            {
                p.ID,
                p.Cena,
                p.Kolicina,
                alijasProdavnice = p.ProdavnicaSpoj1.Naziv,
                alijasProizvoda = p.ProizvodSpoj1.Ime
            })
            .ToListAsync();
            return Ok(proizvodiProdavnice);
        }

        [EnableCors("CORS")]
        [Route("PreuzmiImenaVeceOdNula/{nazivP}")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiImenaVeceOdNula(string nazivP)
        {
            var proizvodiProdavnice = await Context.ProdavnicaProizvod
            //.Where(p => p.Kolicina > 0)
            .Include(p => p.ProdavnicaSpoj1)
            .Where(p => p.ProdavnicaSpoj1.Naziv == nazivP)
            .Include(p => p.ProizvodSpoj1)
            .Select(p => new
            {
                p.ID,
                p.Cena,
                p.Kolicina,
                alijasProdavnice = p.ProdavnicaSpoj1.Naziv,
                alijasProizvoda = p.ProizvodSpoj1.Ime
            })
            .ToListAsync();
            return Ok(proizvodiProdavnice);
        }

        [EnableCors("CORS")]
        [Route("PreuzmiImena/{idP}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiImena(int idP)
        {
            var proizvodiProdavnice = await Context.ProdavnicaProizvod
            .Include(p => p.ProdavnicaSpoj1)
            .Where(p => p.ProdavnicaSpoj1.ID == idP)
            .Include(p => p.ProizvodSpoj1)
            .Select(p => new
            {
                p.ID,
                p.Cena,
                p.Kolicina,
                alijasProdavnice = p.ProdavnicaSpoj1.Naziv,
                alijasProizvoda = p.ProizvodSpoj1.Ime
            })
            .ToListAsync();
            return Ok(proizvodiProdavnice);
        }

        [EnableCors("CORS")]
        [Route("DodajProizvodUProdavnicu/{idP}/{imeProizvoda}/{kolicina}/{cena}")]
        [HttpPost]
        public async Task<ActionResult> DodajProizvodUProdavnicu(int idP, string imeProizvoda, int kolicina, int cena)
        {
            if (idP < 0)
            {
                return BadRequest("Nevalidan ID");
            }
            if (string.IsNullOrWhiteSpace(imeProizvoda) || imeProizvoda.Length > 20)
            {
                return BadRequest("Nevalidno ime proizvoda");
            }
            if (kolicina < 1)
            {
                return BadRequest("Nevalidna kolicina proizvoda");
            }
            if (cena < 1)
            {
                return BadRequest("Nevalidna cena proizvoda");
            }
            try
            {
                var prodavnica = Context.Prodavnice.Where(p => p.ID == idP).FirstOrDefault();
                var proizvod = Context.Proizvodi.Where(p => p.Ime == imeProizvoda).FirstOrDefault();
                if (prodavnica != null && proizvod != null)
                {
                    ProdavnicaProizvodSpoj ovoDodaj = new ProdavnicaProizvodSpoj
                    {
                        Kolicina = kolicina,
                        Cena = cena,
                        ProdavnicaSpoj1 = prodavnica,
                        ProizvodSpoj1 = proizvod
                    };
                    Context.ProdavnicaProizvod.Add(ovoDodaj);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno dodat proizvod u prodavnicu");
                }
                else
                {
                    return BadRequest("Ne postoji prodavnica ili proizvod");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzbrisiProizvodUProdavnici/{idP}/{idPr}")]
        [HttpDelete]

        public async Task<ActionResult> IzbrisiProizvodUProdavnici(int idP, int idPr)
        {
            if (idP < 0 || idPr < 0)
            {
                return BadRequest("Nevalidan ID");
            }
            try
            {
                var prodavnica = Context.Prodavnice.Where(p => p.ID == idP).FirstOrDefault();
                var proizvod = Context.Proizvodi.Where(p => p.ID == idPr).FirstOrDefault();
                if (prodavnica != null && proizvod != null)
                {
                    var prodavnicaProizvod = Context.ProdavnicaProizvod.Where(p => p.ProdavnicaSpoj1 == prodavnica && p.ProizvodSpoj1 == proizvod).FirstOrDefault();
                    if (prodavnicaProizvod != null)
                    {
                        Context.ProdavnicaProizvod.Remove(prodavnicaProizvod);
                        await Context.SaveChangesAsync();
                        return Ok($"Uspesno izbrisan proizvod u prodavnici");
                    }
                    else
                    {
                        return BadRequest("Ne postoji dati proizvod u prodavnici");
                    }
                }
                else
                {
                    return BadRequest("Prodavnica ili proizvod nije pronadjena");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzmeniProizvodUProdavnici/{idP}/{imeP}/{kol}/{cena}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniProizvodUProdavnici(int idP, string imeP, int kol, int cena)
        {
            if (string.IsNullOrEmpty(imeP) || imeP.Length > 20)
            {
                return BadRequest("Nevalidna imena");
            }
            if (idP < 0)
            {
                return BadRequest("Nevalidan id");
            }
            if (kol < 1)
            {
                return BadRequest("Nevalidna kolicina proizvoda");
            }
            if (cena < 1)
            {
                return BadRequest("Nevalidna cena proizvoda");
            }
            try
            {
                var prodavnica = Context.Prodavnice.Where(p => p.ID == idP).FirstOrDefault();
                var proizvod = Context.Proizvodi.Where(p => p.Ime == imeP).FirstOrDefault();
                if (prodavnica != null && proizvod != null)
                {
                    var prodavnicaProizvod = Context.ProdavnicaProizvod.Where(p => p.ProdavnicaSpoj1 == prodavnica && p.ProizvodSpoj1 == proizvod).FirstOrDefault();
                    if (prodavnicaProizvod != null)
                    {
                        prodavnicaProizvod.Kolicina = kol;
                        prodavnicaProizvod.Cena = cena;
                        await Context.SaveChangesAsync();
                        return Ok("Uspesno izmenjen proizvod u prodavnici");
                    }
                    else
                    {
                        return BadRequest("Ne postoji dati proizvod u prodavnici");
                    }
                }
                else
                {
                    return BadRequest("Prodavnica ili proizvod nije pronadjena");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzmeniProizvodUProdavnici/{nazivP}/{imeP}/{kol}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniProizvodUProdavnici(string nazivP, string imeP, int kol)
        {
            if (string.IsNullOrEmpty(nazivP) || string.IsNullOrEmpty(imeP) || nazivP.Length > 20 || imeP.Length > 20)
            {
                return BadRequest("Nevalidna imena");
            }
            try
            {
                var prodavnica = Context.Prodavnice.Where(p => p.Naziv == nazivP).FirstOrDefault();
                var proizvod = Context.Proizvodi.Where(p => p.Ime == imeP).FirstOrDefault();
                if (prodavnica != null && proizvod != null)
                {
                    var prodavnicaProizvod = Context.ProdavnicaProizvod.Where(p => p.ProdavnicaSpoj1 == prodavnica && p.ProizvodSpoj1 == proizvod).FirstOrDefault();
                    if (prodavnicaProizvod != null)
                    {
                        prodavnicaProizvod.Kolicina = prodavnicaProizvod.Kolicina + kol;
                        await Context.SaveChangesAsync();
                        return Ok("Uspesno izmenjen proizvod u prodavnici");
                    }
                    else
                    {
                        return BadRequest("Ne postoji dati proizvod u prodavnici");
                    }
                }
                else
                {
                    return BadRequest("Prodavnica ili proizvod nije pronadjena");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
