using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;

namespace API.Controllers
{
    [Authorize]
    public class MonedasController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Monedas
        public IQueryable<Moneda> GetMoneda()
        {
            return db.Moneda;
        }

        // GET: api/Monedas/5
        [ResponseType(typeof(Moneda))]
        public IHttpActionResult GetMoneda(int id)
        {
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return NotFound();
            }

            return Ok(moneda);
        }

        // PUT: api/Monedas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMoneda(int id, Moneda moneda)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != moneda.Codigo)
            {
                return BadRequest();
            }

            db.Entry(moneda).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonedaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Monedas
        [ResponseType(typeof(Moneda))]
        public IHttpActionResult PostMoneda(Moneda moneda)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Moneda.Add(moneda);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = moneda.Codigo }, moneda);
        }

        // DELETE: api/Monedas/5
        [ResponseType(typeof(Moneda))]
        public IHttpActionResult DeleteMoneda(int id)
        {
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return NotFound();
            }

            db.Moneda.Remove(moneda);
            db.SaveChanges();

            return Ok(moneda);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MonedaExists(int id)
        {
            return db.Moneda.Count(e => e.Codigo == id) > 0;
        }
    }
}