using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK
{
    [Table("MATERIA_PRIMA", Schema = "dbo")]
    public partial class MateriaPrima
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MAPR_ID")]
        public int MaprId { get; set; }

        [Column("MAPR_NOMBRE")]
        [Required]
        public string MaprNombre { get; set; }

        public ICollection<Stock> Stocks { get; set; }
    }
}