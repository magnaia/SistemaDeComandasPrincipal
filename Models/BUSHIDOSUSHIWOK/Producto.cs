using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("PRODUCTO", Schema = "dbo")]
    public partial class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PRO_ID")]
        public int ProId { get; set; }

        [Column("PRO_NOMBRE")]
        public string ProNombre { get; set; }

        [Column("PRO_DESCRIPCION")]
        public string ProDescripcion { get; set; }

        [Column("PRO_VALOR")]
        public int? ProValor { get; set; }

        [Column("PRO_ESTADO")]
        public string ProEstado { get; set; }

        [Column("PRO_TIPO")]
        public int? ProTipo { get; set; }

        public TipoPlato TipoPlato { get; set; }

        [Column("PRO_CANTIDAD")]
        public int? ProCantidad { get; set; }

        public ICollection<DetalleProducto> DetalleProductos { get; set; }

        public ICollection<Orden> Ordens { get; set; }
    }
}