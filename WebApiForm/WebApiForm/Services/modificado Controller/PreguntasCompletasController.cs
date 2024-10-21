using Microsoft.AspNetCore.Mvc;
using WebApiForm.Repository;

namespace WebApiForm.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreguntasCompletasController : ControllerBase
    {
        private readonly FormEncuestaDbContext _context;

        public PreguntasCompletasController(FormEncuestaDbContext context)
        {
            _context = context;
        }

        [HttpGet("obtenerPreguntasCompleto")]
        public async Task<ActionResult<IEnumerable<PreguntaCompleta>>> ObtenerPreguntasCompleto()
        {
            var preguntasCompleto = await _context.GetPreguntasCompleto();
            if (preguntasCompleto == null || preguntasCompleto.Count == 0)
            {
                return NotFound("No se encontraron preguntas completas.");
            }
            return Ok(preguntasCompleto);
        }
    }
}
