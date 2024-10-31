using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("DETALLE_ROLL", Schema = "dbo")]
    public partial class DetalleRoll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ROLL_ID")]
        public int RollId { get; set; }

        [Column("ROLL_INGREDIENTE")]
        public int? RollIngrediente { get; set; }

        public Ingrediente Ingrediente { get; set; }

        [Column("ROLL_DETPRODUCTO")]
        public int? RollDetproducto { get; set; }

        public DetalleProducto DetalleProducto { get; set; }
    }
}