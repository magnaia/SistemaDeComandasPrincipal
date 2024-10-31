using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("TRABAJADOR", Schema = "dbo")]
    public partial class Trabajador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TRA_ID")]
        public int TraId { get; set; }

        [Column("TRA_NOMBRE")]
        [Required]
        public string TraNombre { get; set; }

        [Column("TRA_APELLIDO1")]
        [Required]
        public string TraApellido1 { get; set; }

        [Column("TRA_APELLIDO2")]
        [Required]
        public string TraApellido2 { get; set; }

        [Column("TRA_CARGO")]
        [Required]
        public int TraCargo { get; set; }

        public Cargo Cargo { get; set; }

        [Column("TRA_SUCURSAL")]
        [Required]
        public int TraSucursal { get; set; }

        public Sucursal Sucursal { get; set; }

        [Column("TRA_RUN")]
        public string TraRun { get; set; }

        [Column("TRA_AFP")]
        public string TraAfp { get; set; }

        [Column("TRA_ESTADO")]
        public string TraEstado { get; set; }
    }
}