using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiForm.Repository.Models;

[Table("EncuestaOrden")]
public partial class EncuestaOrden
{
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string Year { get; set; } = null!;

    public int? OrderNumber { get; set; }
}
