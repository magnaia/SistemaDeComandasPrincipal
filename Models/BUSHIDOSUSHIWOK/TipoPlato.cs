using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("TIPO_PLATO", Schema = "dbo")]
    public partial class TipoPlato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TIPLA_ID")]
        public int TiplaId { get; set; }

        [Column("TIPLA_NOMBRE")]
        [Required]
        public string TiplaNombre { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}