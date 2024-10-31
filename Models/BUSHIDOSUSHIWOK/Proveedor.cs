using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("PROVEEDOR", Schema = "dbo")]
    public partial class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROV_ID")]
        public int ProvId { get; set; }

        [Column("PROV_RUN")]
        [Required]
        public int ProvRun { get; set; }

        [Column("PROV_NOMBRE")]
        [Required]
        public string ProvNombre { get; set; }

        [Column("PROV_ESTADO")]
        [Required]
        public string ProvEstado { get; set; }

        [Column("PROV_CORREO")]
        [Required]
        public string ProvCorreo { get; set; }

        public ICollection<CompraInsumo> CompraInsumos { get; set; }
    }
}