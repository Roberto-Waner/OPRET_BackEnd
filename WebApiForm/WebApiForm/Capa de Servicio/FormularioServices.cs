using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using WebApiForm.DTO__Data_Transfer_Object_;
using WebApiForm.Repository;

namespace WebApiForm.Capa_de_Servicio
{
    public class FormularioServices
    {
        private readonly FormEncuestaDbContext _context;

        public FormularioServices(FormEncuestaDbContext context)
        {
            _context = context;
        }

        public async Task<List<ObtenerForm_Dto>> ObtenerFormularioAsyncService()
        {
            return await _context.ObtenerFormularioAsync();
        }

        public async Task<List<FiltrarFormularios_Dto>> FiltrarFormularioAsyncServices(string filtrar) //no esta en uso actualmente
        {
            var query = "EXEC sp_FiltrarFormulario @Filtro = {0}";
            return await _context.filtrarFormulariosDtos.FromSqlRaw(query, filtrar).ToListAsync();
        }
    }
}
