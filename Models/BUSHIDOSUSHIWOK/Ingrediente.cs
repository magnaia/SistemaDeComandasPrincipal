using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("INGREDIENTE", Schema = "dbo")]
    public partial class Ingrediente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ING_ID")]
        public int IngId { get; set; }

        [Column("ING_NOMBRE")]
        public int? IngNombre { get; set; }

        [Column("ING_COSTO")]
        public int? IngCosto { get; set; }

        [Column("ING_TIPO")]
        public int? IngTipo { get; set; }

        public TipoIngrediente TipoIngrediente { get; set; }

        public ICollection<DetalleRoll> DetalleRolls { get; set; }
    }
}