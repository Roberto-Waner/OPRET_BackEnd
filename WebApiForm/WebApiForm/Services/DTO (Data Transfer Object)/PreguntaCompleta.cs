﻿namespace WebApiForm.Services
{
    public class PreguntaCompleta
    {
        public int CodPregunta { get; set; }
        public string? TipoRespuesta { get; set; }
        public string Pregunta { get; set; }
        public string? SubPregunta { get; set; }
        public string? Rango { get; set; }
    }
}