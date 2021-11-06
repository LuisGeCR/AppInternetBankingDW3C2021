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
    public class SesionsController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Sesions
        public IQueryable<Sesion> GetSesion()
        {
            return db.Sesion;
        }

        // GET: api/Sesions/5
        [ResponseType(typeof(Sesion))]
        public IHttpActionResult GetSesion(int id)
        {
            Sesion sesion = db.Sesion.Find(id);
            if (sesion == null)
            {
                return NotFound();
            }

            return Ok(sesion);
        }

        // PUT: api/Sesions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSesion(int id, Sesion sesion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sesion.Codigo)
            {
                return BadRequest();
            }

            db.Entry(sesion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SesionExists(id))
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

        // POST: api/Sesions
        [ResponseType(typeof(Sesion))]
        public IHttpActionResult PostSesion(Sesion sesion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sesion.Add(sesion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sesion.Codigo }, sesion);
        }

        // DELETE: api/Sesions/5
        [ResponseType(typeof(Sesion))]
        public IHttpActionResult DeleteSesion(int id)
        {
            Sesion sesion = db.Sesion.Find(id);
            if (sesion == null)
            {
                return NotFound();
            }

            db.Sesion.Remove(sesion);
            db.SaveChanges();

            return Ok(sesion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SesionExists(int id)
        {
            return db.Sesion.Count(e => e.Codigo == id) > 0;
        }
    }
}