using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("CARGO", Schema = "dbo")]
    public partial class Cargo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CAR_ID")]
        public int CarId { get; set; }

        [Column("CAR_TIPO")]
        [Required]
        public int CarTipo { get; set; }

        [Column("CAR_SUELDO")]
        [Required]
        public int CarSueldo { get; set; }

        public ICollection<Trabajador> Trabajadors { get; set; }
    }
}