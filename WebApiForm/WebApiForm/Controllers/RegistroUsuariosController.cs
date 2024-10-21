using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiForm.Capa_de_Servicio;
using WebApiForm.Repository;
using WebApiForm.Repository.Models;
using WebApiForm.Services.DTO__Data_Transfer_Object_;

namespace WebApiForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroUsuariosController : ControllerBase
    {
        private readonly FormEncuestaDbContext _context;
        private readonly EmpleadoService _empleadoService;

        public RegistroUsuariosController(FormEncuestaDbContext context, EmpleadoService empleadoService)
        {
            _context = context;
            _empleadoService = empleadoService;
        }

        // GET: api/RegistroUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroUsuario>>> GetRegistroUsuarios()
        {
            return await _context.RegistroUsuarios.ToListAsync();
        }

        // GET: api/RegistroUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroUsuario>> GetRegistroUsuario(string id)
        {
            var registroUsuario = await _context.RegistroUsuarios.FindAsync(id);

            if (registroUsuario == null)
            {
                return NotFound();
            }

            return registroUsuario;
        }

        // PUT: api/RegistroUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroUsuario(string id, RegistroUsuario registroUsuario)
        {
            if (id != registroUsuario.IdUsuarios)
            {
                return BadRequest();
            }

            _context.Entry(registroUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroUsuarioExists(id))
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

        // POST: api/RegistroUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RegistroUsuario>> PostRegistroUsuario(RegistroUsuario registroUsuario)
        {
            _context.RegistroUsuarios.Add(registroUsuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RegistroUsuarioExists(registroUsuario.IdUsuarios))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRegistroUsuario", new { id = registroUsuario.IdUsuarios }, registroUsuario);
        }

        // DELETE: api/RegistroUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroUsuario(string id)
        {
            var registroUsuario = await _context.RegistroUsuarios.FindAsync(id);
            if (registroUsuario == null)
            {
                return NotFound();
            }

            _context.RegistroUsuarios.Remove(registroUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroUsuarioExists(string id)
        {
            return _context.RegistroUsuarios.Any(e => e.IdUsuarios == id);
        }

        [HttpGet("ObtenerEmpl")]
        public async Task<ActionResult<List<ObtenerEmpleados>>> getObtenerEmpleados()
        {
            var empleados = await _empleadoService._ObtenerEmpleadosAsync();
            return Ok(empleados);
        }
    }
}
