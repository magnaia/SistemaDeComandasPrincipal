using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("BOLETA", Schema = "dbo")]
    public partial class Boletum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("BOL_ID")]
        public int BolId { get; set; }

        [Column("BOL_FECHA")]
        public DateTime? BolFecha { get; set; }

        [Column("BOL_SUCURSAL")]
        public int? BolSucursal { get; set; }

        public Sucursal Sucursal { get; set; }

        [Column("BOL_CLI_NOMBRE")]
        public string BolCliNombre { get; set; }

        [Column("BOL_CLI_APELLIDO")]
        public string BolCliApellido { get; set; }

        [Column("BOL_CLI_RUT")]
        public string BolCliRut { get; set; }

        [Column("BOL_CLI_TELEFONO")]
        public string BolCliTelefono { get; set; }

        [Column("BOL_CLI_DIRECCION")]
        public string BolCliDireccion { get; set; }

        [Column("BOL_TIPO_PAGO")]
        public int? BolTipoPago { get; set; }

        public TipoPago TipoPago { get; set; }

        [Column("BOL_SUBTOTAL")]
        public int? BolSubtotal { get; set; }

        [Column("BOL_TOTAL")]
        public int? BolTotal { get; set; }

        public ICollection<Orden> Ordens { get; set; }
    }
}