using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("VEHICULO", Schema = "dbo")]
    public partial class Vehiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("VEHI_ID")]
        public int VehiId { get; set; }

        [Column("VEHI_PATENTE")]
        public string VehiPatente { get; set; }

        [Column("VEHI_MARCA")]
        public string VehiMarca { get; set; }

        [Column("VEHI_SUCURSAL")]
        public int? VehiSucursal { get; set; }

        public Sucursal Sucursal { get; set; }

        [Column("VEHI_ESTADO")]
        public string VehiEstado { get; set; }
    }
}