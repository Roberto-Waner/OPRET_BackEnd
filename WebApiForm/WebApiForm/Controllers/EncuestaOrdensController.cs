using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiForm.Repository;
using WebApiForm.Repository.Models;

namespace WebApiForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncuestaOrdensController : ControllerBase
    {
        private readonly FormEncuestaDbContext _context;

        public EncuestaOrdensController(FormEncuestaDbContext context)
        {
            _context = context;
        }

        // GET: api/EncuestaOrdens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EncuestaOrden>>> GetEncuestaOrdens()
        {
            return await _context.EncuestaOrdens.ToListAsync();
        }

        // GET: api/EncuestaOrdens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EncuestaOrden>> GetEncuestaOrden(string id)
        {
            var encuestaOrden = await _context.EncuestaOrdens.FindAsync(id);

            if (encuestaOrden == null)
            {
                return NotFound();
            }

            return encuestaOrden;
        }

        // PUT: api/EncuestaOrdens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEncuestaOrden(string id, EncuestaOrden encuestaOrden)
        {
            if (id != encuestaOrden.Year)
            {
                return BadRequest();
            }

            _context.Entry(encuestaOrden).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EncuestaOrdenExists(id))
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

        // POST: api/EncuestaOrdens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EncuestaOrden>> PostEncuestaOrden(EncuestaOrden encuestaOrden)
        {
            _context.EncuestaOrdens.Add(encuestaOrden);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EncuestaOrdenExists(encuestaOrden.Year))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEncuestaOrden", new { id = encuestaOrden.Year }, encuestaOrden);
        }

        // DELETE: api/EncuestaOrdens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEncuestaOrden(string id)
        {
            var encuestaOrden = await _context.EncuestaOrdens.FindAsync(id);
            if (encuestaOrden == null)
            {
                return NotFound();
            }

            _context.EncuestaOrdens.Remove(encuestaOrden);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EncuestaOrdenExists(string id)
        {
            return _context.EncuestaOrdens.Any(e => e.Year == id);
        }
    }
}
