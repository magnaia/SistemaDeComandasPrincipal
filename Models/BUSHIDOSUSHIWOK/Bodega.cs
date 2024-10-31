using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("BODEGA", Schema = "dbo")]
    public partial class Bodega
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("BOD_ID")]
        public int BodId { get; set; }

        [Column("BOD_SUCURSAL")]
        public int? BodSucursal { get; set; }

        public Sucursal Sucursal { get; set; }

        public ICollection<CompraInsumo> CompraInsumos { get; set; }

        public ICollection<Stock> Stocks { get; set; }
    }
}