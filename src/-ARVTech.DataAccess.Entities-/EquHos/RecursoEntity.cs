namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("RECURSOS")]
    public class RecursoEntity
    {
        [Key]
        public int? Id { get; set; }

        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Column("SITUACAO")]
        public string Situacao { get; set; } = "A";
    }
}