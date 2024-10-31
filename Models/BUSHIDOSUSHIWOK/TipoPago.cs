using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("TIPO_PAGO", Schema = "dbo")]
    public partial class TipoPago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TIPA_ID")]
        public int TipaId { get; set; }

        [Column("TIPA_NOMBRE")]
        [Required]
        public string TipaNombre { get; set; }

        public ICollection<Boletum> Boleta { get; set; }
    }
}