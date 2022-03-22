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
    public class DostavljacController : ControllerBase
    {
        public OnlineProdavnicaContext Context { get; set; }

        public DostavljacController(OnlineProdavnicaContext context)
        {
            Context = context;
        }

        [EnableCors("CORS")]
        [Route("PreuzmiDostavljace")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiDostavljace()
        {
            var dostavljaci = await Context.Dostavljaci
            .Include(p => p.DostavljacProdavnica)
            .Select(p => new
            {
                p.ID,
                p.Ime,
                p.Prezime,
                p.Godine,
                p.BrTelefona,
                p.CenaUsluge,
                alijasProdavnice = p.DostavljacProdavnica.Naziv
            }).ToListAsync();
            return Ok(dostavljaci);
        }

        [EnableCors("CORS")]
        [Route("PreuzmiDostavljace/{idP}")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiDostavljace(int idP)
        {
            var dostavljaci = await Context.Dostavljaci
            .Include(p => p.DostavljacProdavnica)
            .Where(p => p.DostavljacProdavnica.ID == idP)
            .Select(p => new
            {
                p.ID,
                p.Ime,
                p.Prezime,
                p.Godine,
                p.BrTelefona,
                p.CenaUsluge,
                alijasProdavnice = p.DostavljacProdavnica.Naziv
            }).ToListAsync();
            return Ok(dostavljaci);
        }

        [EnableCors("CORS")]
        [Route("DodajDostavljaca")]
        [HttpPost]
        public async Task<ActionResult> DodajDostavljaca([FromBody] Dostavljac dostavljac)
        {
            if (string.IsNullOrWhiteSpace(dostavljac.Ime) || dostavljac.Ime.Length > 20)
            {
                return BadRequest("Nevalidno ime");
            }
            if (string.IsNullOrWhiteSpace(dostavljac.Prezime) || dostavljac.Prezime.Length > 20)
            {
                return BadRequest("Nevalidno prezime");
            }
            if (string.IsNullOrWhiteSpace(dostavljac.BrTelefona))
            {
                return BadRequest("Nevalidan broj telefona");
            }
            if (dostavljac.Godine < 18 || dostavljac.Godine > 60)
            {
                return BadRequest("Nevalidne godine");
            }
            if (dostavljac.CenaUsluge < 100 || dostavljac.CenaUsluge > 1000)
            {
                return BadRequest("Nevalidna cena usluge");
            }
            try
            {
                var d = Context.Dostavljaci.Where(p => p.BrTelefona == dostavljac.BrTelefona).FirstOrDefault();
                if (d == null)
                {
                    Context.Dostavljaci.Add(dostavljac);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno dodat dostavljac");
                }
                else
                {
                    return BadRequest("Vec postoji dati dostavaljac");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("DodajDostavljaca/{ime}/{prezime}/{brtelefona}/{godine}/{cena}/{idP}")]
        [HttpPost]
        public async Task<ActionResult> DodajDostavljaca(string ime, string prezime, string brtelefona, int godine, int cena, int idP)
        {
            if (string.IsNullOrWhiteSpace(ime) || ime.Length > 20)
            {
                return BadRequest("Nevalidno ime");
            }
            if (string.IsNullOrWhiteSpace(prezime) || prezime.Length > 20)
            {
                return BadRequest("Nevalidno prezime");
            }
            if (string.IsNullOrWhiteSpace(brtelefona))
            {
                return BadRequest("Nevalidan broj telefona");
            }
            if (godine < 18 || godine > 60)
            {
                return BadRequest("Nevalidne godine");
            }
            if (cena < 100 || cena > 1000)
            {
                return BadRequest("Nevalidna cena usluge");
            }
            if (idP < 0)
            {
                return BadRequest("Nevalidan id");
            }
            try
            {
                var d = Context.Dostavljaci.Where(p => p.BrTelefona == brtelefona).FirstOrDefault();
                if (d == null)
                {
                    var prodavnica = Context.Prodavnice.Where(p => p.ID == idP).FirstOrDefault();
                    var dostavaljac = new Dostavljac
                    {
                        Ime = ime,
                        Prezime = prezime,
                        BrTelefona = brtelefona,
                        Godine = godine,
                        CenaUsluge = cena,
                        DostavljacProdavnica = prodavnica
                    };
                    Context.Dostavljaci.Add(dostavaljac);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno dodat dostavljac");
                }
                else
                {
                    return BadRequest("Vec postoji dati dostavaljac");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("DodajDostavljacaProdavnici/{idD}/{idP}")]
        [HttpPost]
        public async Task<ActionResult> DodajDostavljacaProdavnici(int idD, int idP)
        {
            if (idD < 0 || idP < 0)
            {
                return BadRequest("Nevalidan id");
            }
            try
            {
                var dostavljac = Context.Dostavljaci.Where(p => p.ID == idD).FirstOrDefault();
                var prodavnica = Context.Prodavnice.Where(p => p.ID == idP).FirstOrDefault();
                if (dostavljac != null && prodavnica != null)
                {
                    dostavljac.DostavljacProdavnica = prodavnica;
                }
                else
                {
                    return BadRequest("Nije pronadjen dostavljac ili prodavnica");
                }
                await Context.SaveChangesAsync();
                return Ok("Uspesno dodat dostavljac");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzbrisiDostavljaca/{id}")]
        [HttpDelete]

        public async Task<ActionResult> IzbrisiDostavljaca(int id)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan id");
            }
            try
            {
                var dostavljac = Context.Dostavljaci.Where(p => p.ID == id).FirstOrDefault();
                if (dostavljac != null)
                {
                    Context.Dostavljaci.Remove(dostavljac);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izbirisan dostavljac");
                }
                else
                {
                    return BadRequest("Dostavljac nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzmeniDostavljaca/{id}/{ime}/{prezime}/{godine}/{brtelefona}/{cena}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniProdavnicu(int id, string ime, string prezime, int godine, string brtelefona, int cena)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan ID");
            }
            if (string.IsNullOrWhiteSpace(ime) || ime.Length > 20)
            {
                return BadRequest("Nevalidno ime");
            }
            if (string.IsNullOrWhiteSpace(prezime) || prezime.Length > 20)
            {
                return BadRequest("Nevalidno prezime");
            }
            if (string.IsNullOrWhiteSpace(brtelefona))
            {
                return BadRequest("Nevalidna adresa");
            }
            try
            {
                var dostavljac = Context.Dostavljaci.Where(p => p.ID == id).FirstOrDefault();
                if (dostavljac != null)
                {
                    dostavljac.Ime = ime;
                    dostavljac.Prezime = prezime;
                    dostavljac.Godine = godine;
                    dostavljac.BrTelefona = brtelefona;
                    dostavljac.CenaUsluge = cena;
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izmenanjen dostavljac");
                }
                else
                {
                    return BadRequest("Dostavljac nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
