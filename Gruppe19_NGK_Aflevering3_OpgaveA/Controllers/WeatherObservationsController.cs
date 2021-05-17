using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Gruppe19_NGK_Aflevering3_OpgaveA.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gruppe19_NGK_Aflevering3_OpgaveA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Gruppe19_NGK_Aflevering3_OpgaveA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherObservationsController : ControllerBase
    {
        private readonly IHubContext<WeatherObservationHub> _WeatherObservationHubContext;
        private readonly MyDbContext _context;

        public WeatherObservationsController(MyDbContext context, IHubContext<WeatherObservationHub> hub)
        {
            _context = context;
            _WeatherObservationHubContext = hub;
        }
        /// <summary>
        /// Get all weatherobservations
        /// </summary>
        /// <returns> Array of all weatherobservation in JSON format</returns>
        // GET: api/WeatherObservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherObservation>>> GetWeatherObservation()
        {
            return await _context.WeatherObservation.Include(w=>w.Location).ToListAsync();
        }
        /// <summary>
        /// Gets the 3 newly added weatherobservations
        /// </summary>
        /// <returns> Returns the 3 latest weatherobservations in JSON format </returns>
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

        /// <summary>
        /// Returns weatherobservations by the date
        /// </summary>
        /// <param name="date"> The date with form YYYY-MM-DD</param>
        /// <returns> The weather observations from the date </returns>
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
        /// <summary>
        /// Returns the weatherobservations between the 2 supplied dates
        /// </summary>
        /// <param name="startdate"> Startdate with form YYYY-MM-DD</param>
        /// <param name="endDate"> Enddate with form YYYY-MM-DD</param>
        /// <returns> Returns array of weatherobservations in JSON format </returns>
        // GET: api/WeatherObservations
        [HttpGet("/BetweenDates")]
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
        /// <summary>
        /// Returns weatherobservation by id
        /// </summary>
        /// <param name="id"> The id for the weatherobservation</param>
        /// <returns> The weatherobservation with the give id </returns>
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
        [Authorize]
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
        /// <summary>
        /// Adds a weatherobservation, if the user is authorized
        /// </summary>
        /// <param name="weatherObservation"> Object corresponding to the weatherobservation class</param>
        /// <returns> The added observation if the data corresponds to the weatherobservation class</returns>
        // POST: api/WeatherObservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WeatherObservation>> PostWeatherObservation(WeatherObservation weatherObservation)
        {
            _context.WeatherObservation.Add(weatherObservation);
            await _context.SaveChangesAsync();

            string JSON = JsonSerializer.Serialize(weatherObservation);
            await _WeatherObservationHubContext.Clients.All.SendAsync("SendObservation",JSON);

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
