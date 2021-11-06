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
    public class CreditosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Creditos
        public IQueryable<Credito> GetCredito()
        {
            return db.Credito;
        }

        // GET: api/Creditos/5
        [ResponseType(typeof(Credito))]
        public IHttpActionResult GetCredito(int id)
        {
            Credito credito = db.Credito.Find(id);
            if (credito == null)
            {
                return NotFound();
            }

            return Ok(credito);
        }

        // PUT: api/Creditos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCredito(int id, Credito credito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != credito.Codigo)
            {
                return BadRequest();
            }

            db.Entry(credito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditoExists(id))
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

        // POST: api/Creditos
        [ResponseType(typeof(Credito))]
        public IHttpActionResult PostCredito(Credito credito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Credito.Add(credito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = credito.Codigo }, credito);
        }

        // DELETE: api/Creditos/5
        [ResponseType(typeof(Credito))]
        public IHttpActionResult DeleteCredito(int id)
        {
            Credito credito = db.Credito.Find(id);
            if (credito == null)
            {
                return NotFound();
            }

            db.Credito.Remove(credito);
            db.SaveChanges();

            return Ok(credito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CreditoExists(int id)
        {
            return db.Credito.Count(e => e.Codigo == id) > 0;
        }
    }
}