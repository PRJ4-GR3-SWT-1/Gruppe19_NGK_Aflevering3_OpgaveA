using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gruppe19_NGK_Aflevering3_OpgaveA.Models;

namespace Gruppe19_NGK_Aflevering3_OpgaveA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherObservationsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public WeatherObservationsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/WeatherObservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherObservation>>> GetWeatherObservation()
        {
            return await _context.WeatherObservation.Include(w=>w.Location).ToListAsync();
        }

        // GET: api/WeatherObservations
        [HttpGet("/Last3")]
        public async Task<ActionResult<IEnumerable<WeatherObservation>>> GetLastThreeWeatherObservation()
        {
            return await _context.WeatherObservation
                .Include(w => w.Location)
                .OrderByDescending(w=>w.Date)
                .Take(3)
                .ToListAsync();
        }

        // GET: api/WeatherObservations
        [HttpGet("/ByDate")]
        public async Task<ActionResult<IEnumerable<WeatherObservation>>> GetWeatherObservationByDate(DateTime? date)
        {
            if (date == null) return NotFound();

            DateTime RealDate = (DateTime) date;

            return await _context.WeatherObservation
                .Include(w => w.Location)
                .Where(i=>i.Date.Date == RealDate.Date)
                .ToListAsync();
        }

        // GET: api/WeatherObservations
        [HttpGet("/BetweenDate")]
        public async Task<ActionResult<IEnumerable<WeatherObservation>>> GetWeatherObservationBetweenDate(DateTime? startdate, DateTime? endDate)
        {
            if (startdate == null || endDate == null) return NotFound();

            DateTime RealStartDate = (DateTime)startdate;
            DateTime RealEndDate = (DateTime) endDate;

            return await _context.WeatherObservation
                .Include(w => w.Location)
                .Where(i => i.Date.Date >= RealStartDate.Date)
                .Where(i=>i.Date.Date <=RealEndDate.Date)
                .ToListAsync();
        }

        // GET: api/WeatherObservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherObservation>> GetWeatherObservation(int id)
        {
            var weatherObservation = await _context.WeatherObservation
                .Include(w => w.Location)
                .SingleOrDefaultAsync(w=>w.WeatherObservationId == id);

            if (weatherObservation == null)
            {
                return NotFound();
            }

            return weatherObservation;
        }

        // PUT: api/WeatherObservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeatherObservation(int id, WeatherObservation weatherObservation)
        {
            if (id != weatherObservation.WeatherObservationId)
            {
                return BadRequest();
            }

            _context.Entry(weatherObservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherObservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WeatherObservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WeatherObservation>> PostWeatherObservation(WeatherObservation weatherObservation)
        {
            _context.WeatherObservation.Add(weatherObservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeatherObservation", new { id = weatherObservation.WeatherObservationId }, weatherObservation);
        }

        // DELETE: api/WeatherObservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherObservation(int id)
        {
            var weatherObservation = await _context.WeatherObservation.FindAsync(id);
            if (weatherObservation == null)
            {
                return NotFound();
            }

            _context.WeatherObservation.Remove(weatherObservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeatherObservationExists(int id)
        {
            return _context.WeatherObservation.Any(e => e.WeatherObservationId == id);
        }
    }
}
