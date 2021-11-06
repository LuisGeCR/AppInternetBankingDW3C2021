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
    public class TarjetasController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Tarjetas
        public IQueryable<Tarjeta> GetTarjeta()
        {
            return db.Tarjeta;
        }

        // GET: api/Tarjetas/5
        [ResponseType(typeof(Tarjeta))]
        public IHttpActionResult GetTarjeta(int id)
        {
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            return Ok(tarjeta);
        }

        // PUT: api/Tarjetas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTarjeta(int id, Tarjeta tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tarjeta.Codigo)
            {
                return BadRequest();
            }

            db.Entry(tarjeta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaExists(id))
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

        // POST: api/Tarjetas
        [ResponseType(typeof(Tarjeta))]
        public IHttpActionResult PostTarjeta(Tarjeta tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tarjeta.Add(tarjeta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tarjeta.Codigo }, tarjeta);
        }

        // DELETE: api/Tarjetas/5
        [ResponseType(typeof(Tarjeta))]
        public IHttpActionResult DeleteTarjeta(int id)
        {
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            db.Tarjeta.Remove(tarjeta);
            db.SaveChanges();

            return Ok(tarjeta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarjetaExists(int id)
        {
            return db.Tarjeta.Count(e => e.Codigo == id) > 0;
        }
    }
}