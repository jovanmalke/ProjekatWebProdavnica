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
    public class ProizvodController : ControllerBase
    {
        public OnlineProdavnicaContext Context { get; set; }

        public ProizvodController(OnlineProdavnicaContext context)
        {
            Context = context;
        }

        [EnableCors("CORS")]
        [Route("PreuzmiImena")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiImena()
        {
            var proizvodi = await Context.Proizvodi.Select(p => new
            {
                p.ID,
                p.Ime
            }).ToListAsync();
            return Ok(proizvodi);
        }

        [EnableCors("CORS")]
        [Route("DodajProizvod")]
        [HttpPost]
        public async Task<ActionResult> DodajProizvod([FromBody] Proizvod proizvod)
        {
            if (string.IsNullOrWhiteSpace(proizvod.Ime) || proizvod.Ime.Length > 20)
            {
                return BadRequest("Nevalidno ime");
            }
            try
            {
                Context.Proizvodi.Add(proizvod);
                await Context.SaveChangesAsync();
                return Ok("Uspesno dodat proizvod");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzbrisiProizvod/{id}")]
        [HttpDelete]

        public async Task<ActionResult> IzbrisiProizvod(int id)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan id");
            }
            try
            {
                var proizvod = Context.Proizvodi.Where(p => p.ID == id).FirstOrDefault();
                if (proizvod != null)
                {
                    Context.Proizvodi.Remove(proizvod);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izbirisana proizvod");
                }
                else
                {
                    return BadRequest("Proizvod nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzmeniProizvod/{id}/{ime}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniProizvodBody(int id, string ime)
        {
            if (id < 0)
            {
                return BadRequest("Pogresan ID");
            }
            if (string.IsNullOrWhiteSpace(ime) || ime.Length > 20)
            {
                return BadRequest("Nevalidno ime");
            }
            try
            {
                var proizvodZaPromenu = Context.Proizvodi.Where(p => p.ID == id).FirstOrDefault();
                if (proizvodZaPromenu != null)
                {
                    proizvodZaPromenu.Ime = ime;
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izmenanjena proizvod");
                }
                else
                {
                    return BadRequest("Proizvod nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
