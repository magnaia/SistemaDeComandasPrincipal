using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("MUEBLE", Schema = "dbo")]
    public partial class Mueble
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MUEB_ID")]
        public int MuebId { get; set; }

        [Column("MUEB_NOMBRE")]
        public string MuebNombre { get; set; }

        [Column("MUEB_CODIGO")]
        public string MuebCodigo { get; set; }

        [Column("MUEB_MARCA")]
        public string MuebMarca { get; set; }

        [Column("MUEB_SUCURSAL")]
        public int? MuebSucursal { get; set; }

        public Sucursal Sucursal { get; set; }

        [Column("MEB_ESTADO")]
        public string MebEstado { get; set; }
    }
}