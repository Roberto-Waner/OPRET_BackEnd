//using WebApiForm.Repository;

//namespace WebApiForm.Capa_de_Servicio
//{
//    public class FormularioService
//    {
//        private readonly FormEncuestaDbContext _context;

//        public FormularioService(FormEncuestaDbContext context)
//        {
//            _context = context;
//        }

//        public async Task InsertarFormularioAsync(
//            string idUsuarios,
//            string cedula,
//            string? fecha,
//            string? hora,
//            int? idEstacion,
//            string? idLinea,
//            int orderNumber)
//        {
//            await _context.InsertarFormularioAsync(idUsuarios, cedula, fecha, hora, idEstacion!, idLinea!, orderNumber);
//        }
//    }
//}
