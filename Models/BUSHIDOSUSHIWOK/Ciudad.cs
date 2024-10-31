using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("CIUDAD", Schema = "dbo")]
    public partial class Ciudad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CIU_ID")]
        public int CiuId { get; set; }

        [Column("CIU_NOMBRE")]
        [Required]
        public string CiuNombre { get; set; }

        public ICollection<Sucursal> Sucursals { get; set; }
    }
}