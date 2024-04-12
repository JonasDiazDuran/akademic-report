﻿using AkademicReport.Models;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.AsignaturaDto
{
    public class AsignaturaAddDto
    {

        [Required]
        public int? IdConcepto { get; set; }
        public string? Codigo { get; set; }
        public string? Modalida { get; set; }
        [Required]
        public List<TipoCargaDto> TiposCargas { get; set; }
        public string? Nombre { get; set; }
        public string? Horas { get; set; }
        public string? Descripcion { get; set; }
    }
}
