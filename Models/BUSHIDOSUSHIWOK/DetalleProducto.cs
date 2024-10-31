using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("DETALLE_PRODUCTO", Schema = "dbo")]
    public partial class DetalleProducto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DEPRO_ID")]
        public int DeproId { get; set; }

        [Column("DETPRO_PRODUCTO")]
        public int? DetproProducto { get; set; }

        public Producto Producto { get; set; }

        [Column("DETPRO_CANTIDAD")]
        public int? DetproCantidad { get; set; }

        public ICollection<DetalleRoll> DetalleRolls { get; set; }
    }
}