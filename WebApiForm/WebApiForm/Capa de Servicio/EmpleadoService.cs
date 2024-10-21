using WebApiForm.Repository;
using WebApiForm.Services.DTO__Data_Transfer_Object_;

namespace WebApiForm.Capa_de_Servicio
{
    public class EmpleadoService
    {
        private readonly FormEncuestaDbContext _context;

        public EmpleadoService(FormEncuestaDbContext context)
        {
            _context = context;
        }

        public async Task<List<ObtenerEmpleados>> _ObtenerEmpleadosAsync()
        {
            return await _context.ObtenerEmpleadosAsync();
        }
    }
}
