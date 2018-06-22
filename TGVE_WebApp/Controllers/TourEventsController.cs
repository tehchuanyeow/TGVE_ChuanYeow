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
    public class TourEventsController : ApiController
    {
        private TGVE_ChuanYeowEntities db = new TGVE_ChuanYeowEntities();

        // GET: api/TourEvents
        public IQueryable<TourEvent> GetTourEvents()
        {
            return db.TourEvents;
        }

        // GET: api/TourEvents/5
        [ResponseType(typeof(TourEvent))]
        public async Task<IHttpActionResult> GetTourEvent(string id)
        {
            TourEvent tourEvent = await db.TourEvents.FindAsync(id);
            if (tourEvent == null)
            {
                return NotFound();
            }

            return Ok(tourEvent);
        }

        // PUT: api/TourEvents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTourEvent(string id, TourEvent tourEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tourEvent.TourName)
            {
                return BadRequest();
            }

            db.Entry(tourEvent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourEventExists(id))
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

        // POST: api/TourEvents
        [ResponseType(typeof(TourEvent))]
        public async Task<IHttpActionResult> PostTourEvent(TourEvent tourEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TourEvents.Add(tourEvent);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TourEventExists(tourEvent.TourName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tourEvent.TourName }, tourEvent);
        }

        // DELETE: api/TourEvents/5
        [ResponseType(typeof(TourEvent))]
        public async Task<IHttpActionResult> DeleteTourEvent(string id)
        {
            TourEvent tourEvent = await db.TourEvents.FindAsync(id);
            if (tourEvent == null)
            {
                return NotFound();
            }

            db.TourEvents.Remove(tourEvent);
            await db.SaveChangesAsync();

            return Ok(tourEvent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TourEventExists(string id)
        {
            return db.TourEvents.Count(e => e.TourName == id) > 0;
        }
    }
}