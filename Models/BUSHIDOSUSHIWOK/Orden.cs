using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("ORDEN", Schema = "dbo")]
    public partial class Orden
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ORD_ID")]
        public int OrdId { get; set; }

        [Column("ORD_ESTADO")]
        public string OrdEstado { get; set; }

        [Column("ORD_COMENTARIO")]
        public string OrdComentario { get; set; }

        [Column("ORD_BOLETA")]
        public int? OrdBoleta { get; set; }

        public Boletum Boletum { get; set; }

        [Column("ORD_PLATO")]
        public int? OrdPlato { get; set; }

        public Producto Producto { get; set; }
    }
}