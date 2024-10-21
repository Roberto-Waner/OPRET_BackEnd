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
using WebApiForm.Services.modelo_stored_procedured;

namespace WebApiForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormulariosController : ControllerBase
    {
        private readonly FormEncuestaDbContext _context;
        private readonly FormularioService _formularioService;

        public FormulariosController(FormEncuestaDbContext context, FormularioService formularioService)
        {
            _context = context;
            _formularioService = formularioService;
        }

        // GET: api/Formularios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Formulario>>> GetFormularios()
        {
            return await _context.Formularios.ToListAsync();
        }

        // GET: api/Formularios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Formulario>> GetFormulario(int id)
        {
            var formulario = await _context.Formularios.FindAsync(id);

            if (formulario == null)
            {
                return NotFound();
            }

            return formulario;
        }

        // PUT: api/Formularios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormulario(int id, Formulario formulario)
        {
            if (id != formulario.IdentifacadorForm)
            {
                return BadRequest();
            }

            _context.Entry(formulario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormularioExists(id))
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

        // POST: api/Formularios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Formulario>> PostFormulario(Formulario formulario)
        {
            _context.Formularios.Add(formulario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFormulario", new { id = formulario.IdentifacadorForm }, formulario);
        }

        // DELETE: api/Formularios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormulario(int id)
        {
            var formulario = await _context.Formularios.FindAsync(id);
            if (formulario == null)
            {
                return NotFound();
            }

            _context.Formularios.Remove(formulario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormularioExists(int id)
        {
            return _context.Formularios.Any(e => e.IdentifacadorForm == id);
        }

        [HttpPost("Insertar")]
        public async Task<IActionResult> postInsertarForm([FromBody] FormularioDto formulario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _formularioService.InsertarFormularioAsync(
                    formulario.IdUsuarios,
                    formulario.Cedula,
                    formulario.Fecha,
                    formulario.Hora,
                    formulario.IdEstacion,
                    formulario.IdLinea,
                    formulario.OrderNumber);

                return Ok("Formulario insertado correctamente.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar formulario: {ex.Message}");
            }
        }
    }
}
