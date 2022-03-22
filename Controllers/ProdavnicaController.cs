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
    public class ProdavnicaController : ControllerBase
    {
        public OnlineProdavnicaContext Context { get; set; }

        public ProdavnicaController(OnlineProdavnicaContext context)
        {
            Context = context;
        }

        [EnableCors("CORS")]
        [Route("PreuzmiIme")]
        [HttpGet]

        public async Task<ActionResult> PreuzmiImena()
        {
            var prodavnice = await Context.Prodavnice
            .Select(p => new
            {
                p.ID,
                p.Naziv,
                p.Adresa,
                p.ProdavnicaNabavljac
            })
            .ToListAsync();
            return Ok(prodavnice);
        }

        [EnableCors("CORS")]
        [Route("DodajProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica prodavnica)
        {
            if (string.IsNullOrWhiteSpace(prodavnica.Naziv) || prodavnica.Naziv.Length > 20)
            {
                return BadRequest("Nevalidan naziv");
            }
            if (string.IsNullOrWhiteSpace(prodavnica.Adresa) || prodavnica.Adresa.Length > 50)
            {
                return BadRequest("Nevalidna adresa");
            }
            try
            {
                var p = Context.Prodavnice.Where(p => p.Naziv == prodavnica.Naziv).FirstOrDefault();
                if (p == null)
                {
                    Context.Prodavnice.Add(prodavnica);
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno dodata prodavnica");
                }
                else
                {
                    return BadRequest("Vec postoji prodavnica");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("DodajProdavniciNabavljaca/{idP}/{idN}")]
        [HttpPost]
        public async Task<ActionResult> DodajProdavniciNabavljaca(int idP, int idN)
        {
            if (idP < 0 || idN < 0)
            {
                return BadRequest("Nevalidan id prodavnice ili nabavljaca");
            }
            try
            {
                var nabavljac = Context.Nabavljaci.Where(p => p.ID == idN).FirstOrDefault();
                if (nabavljac != null)
                {
                    var prodavnica = Context.Prodavnice.Where(p => p.ID == idP).FirstOrDefault();
                    if (prodavnica != null)
                    {
                        prodavnica.ProdavnicaNabavljac = nabavljac;
                    }
                    else
                    {
                        return BadRequest("Nije pronadjena prodavnica");
                    }
                }
                else
                {
                    return BadRequest("Nije pronadjen nabavljac");
                }
                await Context.SaveChangesAsync();
                return Ok("Uspesno dodat prodavnici nabavljac");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("DodajProdavniciDostavljaca/{idP}/{idD}")]
        [HttpPost]
        public async Task<ActionResult> DodajProdavniciDostavljaca(int idP, int idD)
        {
            if (idP < 0 || idD < 0)
            {
                return BadRequest("Nevalidan id prodavnice ili dostavljaca");
            }
            try
            {
                var dostavljac = Context.Dostavljaci.Where(p => p.ID == idD).FirstOrDefault();
                if (dostavljac != null)
                {
                    var prodavnica = Context.Prodavnice.Where(p => p.ID == idP).FirstOrDefault();
                    if (prodavnica != null && prodavnica.ProdavnicaDostavljaci != null)
                    {
                        prodavnica.ProdavnicaDostavljaci.Add(dostavljac);
                    }
                    else
                    {
                        return BadRequest("Nije pronadjena prodavnica");
                    }
                }
                else
                {
                    return BadRequest("Nije pronadjen nabavljac");
                }
                await Context.SaveChangesAsync();
                return Ok("Uspesno dodat prodavnici dostavljac");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzbrisiProdavnicu/{id}")]
        [HttpDelete]

        public async Task<ActionResult> IzbrisiProdavnicu(int id)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan ID");
            }
            try
            {
                var prodavnica = Context.Prodavnice.Where(p => p.ID == id).FirstOrDefault();
                if (prodavnica != null)
                {
                    Context.Prodavnice.Remove(prodavnica);
                    await Context.SaveChangesAsync();
                    return Ok($"Uspesno izbrisana prodavnica ID: {id}");
                }
                else
                {
                    return BadRequest("Prodavnica nije pronadjena");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("IzmeniProdavnicu/{id}/{naziv}/{adresa}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniProdavnicu(int id, string naziv, string adresa)
        {
            if (id < 0)
            {
                return BadRequest("Nevalidan id");
            }
            if (string.IsNullOrWhiteSpace(naziv) || naziv.Length > 20)
            {
                return BadRequest("Nevalidan naziv");
            }
            if (string.IsNullOrWhiteSpace(adresa) || adresa.Length > 50)
            {
                return BadRequest("Nevalidna adresa");
            }
            try
            {
                var prodavnica = Context.Prodavnice.Where(p => p.ID == id).FirstOrDefault();
                if (prodavnica != null)
                {
                    prodavnica.Naziv = naziv;
                    prodavnica.Adresa = adresa;
                    await Context.SaveChangesAsync();
                    return Ok("Uspesno izmenanjena prodavnica");
                }
                else
                {
                    return BadRequest("Prodavnica nije pronadjen");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
