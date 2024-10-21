using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiForm.Repository.Models;

[Table("Linea")]
public partial class Linea
{
    [Key]
    [Column("id_linea")]
    [StringLength(20)]
    [Unicode(false)]
    public string IdLinea { get; set; } = null!;

    [Column("linea_metro")]
    [StringLength(255)]
    [Unicode(false)]
    public string? LineaMetro { get; set; }
}
