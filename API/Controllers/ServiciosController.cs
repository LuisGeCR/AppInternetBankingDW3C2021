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
    public class ServiciosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Servicios
        public IQueryable<Servicio> GetServicio()
        {
            return db.Servicio;
        }

        // GET: api/Servicios/5
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult GetServicio(int id)
        {
            Servicio servicio = db.Servicio.Find(id);
            if (servicio == null)
            {
                return NotFound();
            }

            return Ok(servicio);
        }

        // PUT: api/Servicios/5
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult PutServicio(Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(servicio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(servicio.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(servicio);
        }

        // POST: api/Servicios
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult PostServicio(Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Servicio.Add(servicio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = servicio.Codigo }, servicio);
        }

        // DELETE: api/Servicios/5
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult DeleteServicio(int id)
        {
            Servicio servicio = db.Servicio.Find(id);
            if (servicio == null)
            {
                return NotFound();
            }

            db.Servicio.Remove(servicio);
            db.SaveChanges();

            return Ok(servicio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServicioExists(int id)
        {
            return db.Servicio.Count(e => e.Codigo == id) > 0;
        }
    }
}