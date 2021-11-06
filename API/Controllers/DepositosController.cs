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
    public class DepositosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Depositos
        public IQueryable<Deposito> GetDeposito()
        {
            return db.Deposito;
        }

        // GET: api/Depositos/5
        [ResponseType(typeof(Deposito))]
        public IHttpActionResult GetDeposito(int id)
        {
            Deposito deposito = db.Deposito.Find(id);
            if (deposito == null)
            {
                return NotFound();
            }

            return Ok(deposito);
        }

        // PUT: api/Depositos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDeposito(int id, Deposito deposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deposito.Codigo)
            {
                return BadRequest();
            }

            db.Entry(deposito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepositoExists(id))
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

        // POST: api/Depositos
        [ResponseType(typeof(Deposito))]
        public IHttpActionResult PostDeposito(Deposito deposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Deposito.Add(deposito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deposito.Codigo }, deposito);
        }

        // DELETE: api/Depositos/5
        [ResponseType(typeof(Deposito))]
        public IHttpActionResult DeleteDeposito(int id)
        {
            Deposito deposito = db.Deposito.Find(id);
            if (deposito == null)
            {
                return NotFound();
            }

            db.Deposito.Remove(deposito);
            db.SaveChanges();

            return Ok(deposito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepositoExists(int id)
        {
            return db.Deposito.Count(e => e.Codigo == id) > 0;
        }
    }
}