using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("SUCURSAL", Schema = "dbo")]
    public partial class Sucursal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SUC_ID")]
        public int SucId { get; set; }

        [Column("SUC_NOMBRE")]
        [Required]
        public string SucNombre { get; set; }

        [Column("SUC_PATENTE")]
        [Required]
        public string SucPatente { get; set; }

        [Column("SUC_CIUDAD")]
        public int? SucCiudad { get; set; }

        public Ciudad Ciudad { get; set; }

        public ICollection<Bodega> Bodegas { get; set; }

        public ICollection<Boletum> Boleta { get; set; }

        public ICollection<Mueble> Muebles { get; set; }

        public ICollection<Repartidor> Repartidors { get; set; }

        public ICollection<Trabajador> Trabajadors { get; set; }

        public ICollection<Vehiculo> Vehiculos { get; set; }
    }
}