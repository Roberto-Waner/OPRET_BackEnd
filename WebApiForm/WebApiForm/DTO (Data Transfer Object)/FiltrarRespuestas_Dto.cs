namespace WebApiForm.DTO__Data_Transfer_Object_
{
    public class FiltrarRespuestas_Dto
    {
        public string? IdUsuarios { get; set; }
        public string? Cedula { get; set; }
        public string? NombreApellido { get; set; }
        public string? NoEncuesta { get; set; }
        public int? IdSesion { get; set; }
        public int? CodPregunta { get; set; }
        public string? Pregunta { get; set; }
        public string? CodSubPregunta { get; set; }
        public string? SubPreguntas { get; set; }
        public string? Respuesta { get; set; }
        public string? Comentarios { get; set; }
        public string? Justificacion { get; set; }
    }
}
