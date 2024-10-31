using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("STOCK", Schema = "dbo")]
    public partial class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("STO_ID")]
        public int StoId { get; set; }

        [Column("STO_MATERIA")]
        public int? StoMateria { get; set; }

        public MateriaPrima MateriaPrima { get; set; }

        [Column("STO_BODEGA")]
        public int? StoBodega { get; set; }

        public Bodega Bodega { get; set; }

        [Column("STO_CANTIDAD")]
        public int? StoCantidad { get; set; }
    }
}