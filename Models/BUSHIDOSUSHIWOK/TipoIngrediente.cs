using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("TIPO_INGREDIENTE", Schema = "dbo")]
    public partial class TipoIngrediente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TIPING_ID")]
        public int TipingId { get; set; }

        [Column("TIPING_NOMBRE")]
        [Required]
        public string TipingNombre { get; set; }

        public ICollection<Ingrediente> Ingredientes { get; set; }
    }
}