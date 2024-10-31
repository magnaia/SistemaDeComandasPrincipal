using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("COMPRA_INSUMO", Schema = "dbo")]
    public partial class CompraInsumo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("COM_ID")]
        public int ComId { get; set; }

        [Column("COM_MATERIA_PRIMA")]
        public int? ComMateriaPrima { get; set; }

        [Column("COM_CANTIDAD")]
        public int? ComCantidad { get; set; }

        [Column("COM_PROVEEDOR")]
        public int? ComProveedor { get; set; }

        public Proveedor Proveedor { get; set; }

        [Column("COM_BODEGA")]
        public int? ComBodega { get; set; }

        public Bodega Bodega { get; set; }

        [Column("COM_FECHA")]
        public DateTime? ComFecha { get; set; }
    }
}