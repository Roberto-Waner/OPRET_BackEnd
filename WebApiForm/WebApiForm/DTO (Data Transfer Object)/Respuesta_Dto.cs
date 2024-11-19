namespace WebApiForm.DTO__Data_Transfer_Object_
{
    public class Respuesta_Dto
    {
        public string IdUsuarios { get; set; }
        public int IdSesion { get; set; }
        public string Respuesta { get; set; }
        public string? Comentarios { get; set; }
        public string? Justificacion { get; set; }
        public int FinalizarSesion { get; set; }
    }
}
