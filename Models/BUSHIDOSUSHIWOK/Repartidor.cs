using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("REPARTIDOR", Schema = "dbo")]
    public partial class Repartidor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("REP_ID")]
        public int RepId { get; set; }

        [Column("REP_NOMBRE")]
        [Required]
        public string RepNombre { get; set; }

        [Column("REP_TELEFONO")]
        [Required]
        public string RepTelefono { get; set; }

        [Column("REP_ESTADO")]
        public string RepEstado { get; set; }

        [Column("REP_SUCURSAL")]
        public int? RepSucursal { get; set; }

        public Sucursal Sucursal { get; set; }
    }
}