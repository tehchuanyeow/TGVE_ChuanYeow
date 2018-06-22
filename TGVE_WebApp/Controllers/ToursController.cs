using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TGVE_WebApp.Models;

namespace TGVE_WebApp.Controllers
{
    public class ToursController : ApiController
    {
        private TGVE_ChuanYeowEntities db = new TGVE_ChuanYeowEntities();

        // GET: api/Tours
        public IQueryable<Tour> GetTours()
        {
            return db.Tours;
        }

        // GET: api/Tours/5
        [ResponseType(typeof(Tour))]
        public async Task<IHttpActionResult> GetTour(string id)
        {
            Tour tour = await db.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }

        // PUT: api/Tours/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTour(string id, Tour tour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tour.TourName)
            {
                return BadRequest();
            }

            db.Entry(tour).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourExists(id))
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

        // POST: api/Tours
        [ResponseType(typeof(Tour))]
        public async Task<IHttpActionResult> PostTour(Tour tour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tours.Add(tour);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TourExists(tour.TourName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tour.TourName }, tour);
        }

        // DELETE: api/Tours/5
        [ResponseType(typeof(Tour))]
        public async Task<IHttpActionResult> DeleteTour(string id)
        {
            Tour tour = await db.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            db.Tours.Remove(tour);
            await db.SaveChangesAsync();

            return Ok(tour);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TourExists(string id)
        {
            return db.Tours.Count(e => e.TourName == id) > 0;
        }
    }
}