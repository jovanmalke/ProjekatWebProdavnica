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
    public class NabavljacController : ControllerBase
    {
        public OnlineProdavnicaContext Context { get; set; }

        public NabavljacController(OnlineProdavnicaContext context)
        {
            Context = context;
        }

        [EnableCors("CORS")]
        [Route("PreuzmiImena")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiImena()
        {
            var nabavljaci = await Context.Nabavljaci.Select(p => new
            {
                p.ID,
                p.Ime,
                p.Prezime,
                p.BrTelefona,
                p.Sifra
            }).ToListAsync();
            return Ok(nabavljaci);
        }

        [EnableCors("CORS")]
        [Route("DodajNabavljaca")]
        [HttpPost]
        public async Task<ActionResult> DodajNabavljaca([FromBody] Nabavljac nabavljac)
        {
            if (string.IsNullOrWhiteSpace(nabavljac.Ime) || nabavljac.Ime.Length > 20)
            {
                return BadRequest("Nevalidno ime");
            }
            if (string.IsNullOrWhiteSpace(nabavljac.Prezime) || nabavljac.Prezime.Length > 20)
            {
                return BadRequest("Nevalidno prezime");
            }
            if (string.IsNullOrWhiteSpace(nabavljac.BrTelefona))
            {
                return BadRequest("Nevalidan broj telefona");
            }
            if (string.IsNullOrWhiteSpace(nabavljac.Sifra) || nabavljac.Sifra.Length > 50)
            {
                return BadRequest("Nevalidna sifra");
            }
            try
            {
                Context.Nabavljaci.Add(nabavljac);
                await Context.SaveChangesAsync();
                return Ok("Uspesno dodat nabavljac");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzbrisiNabavljaca/{id}")]
        [HttpDelete]

        public async Task<ActionResult> IzbrisiDostavljaca(int id)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan id");
            }
            try
            {
                var nabavljac = Context.Nabavljaci.Where(p => p.ID == id).FirstOrDefault();
                if (nabavljac != null)
                {
                    Context.Nabavljaci.Remove(nabavljac);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izbirisan nabavljac");
                }
                else
                {
                    return BadRequest("Nabavljac nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzmeniNabavljaca/{id}/{ime}/{prezime}/{godine}/{brtelefona}/{sifra}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniNabavljaca(int id, string ime, string prezime, int godine, string brtelefona, string sifra)
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
            if (string.IsNullOrWhiteSpace(sifra) || sifra.Length > 50)
            {
                return BadRequest("Nevalidna sifra");
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
