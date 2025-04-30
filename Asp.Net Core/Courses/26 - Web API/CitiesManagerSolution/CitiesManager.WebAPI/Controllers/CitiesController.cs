using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;

namespace CitiesManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return (await _context.Cities.ToListAsync())
                .OrderBy(temp => temp.CityName).ToList();
        }

        // GET: api/Cities/5
        [HttpGet("{cityId}")]
        public async Task<ActionResult<City>> GetCity(Guid cityId)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(temp => temp.CityId == cityId);

            if (city == null)
            {
                return Problem(detail: "Invalid City Id", statusCode: 400, title: "City Search");
                //return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{cityId}")]
        // bind to avoid overposting
        public async Task<IActionResult> PutCity(Guid cityId, [Bind(nameof(City.CityId), nameof(City.CityName))] City city)
        {
            if (cityId != city.CityId)
            {
                return BadRequest();
            }

            var existingCity = await _context.Cities.FindAsync(cityId);
            if (existingCity == null)
            {
                return NotFound();
            }
            existingCity.CityName = city.CityName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(cityId))
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

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<City>> PostCity([Bind(nameof(City.CityId), nameof(City.CityName))] City city)
        {
            //if(ModelState.IsValid == false)
            //{
            //    return ValidationProblem(ModelState);
            //}
            if(_context.Cities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cities' is null");
            }
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { cityId = city.CityId }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(Guid id)
        {
            return _context.Cities.Any(e => e.CityId == id);
        }
    }
}
