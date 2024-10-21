using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiForm.Repository.Models;

[Table("Estacion")]
public partial class Estacion
{
    [Key]
    [Column("id_estacion")]
    public int IdEstacion { get; set; }

    [Column("estacion_metro")]
    [StringLength(255)]
    [Unicode(false)]
    public string? EstacionMetro { get; set; }
}
