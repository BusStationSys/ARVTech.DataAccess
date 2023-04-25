namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTAS")]
    public class ContaEntity
    {
        [Key]
        public Guid? Guid { get; set; }

        public string Cnpj { get; set; }

        [Column("NOME_FANTASIA")]
        public string NomeFantasia { get; set; }

        [Column("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }
    }
}
