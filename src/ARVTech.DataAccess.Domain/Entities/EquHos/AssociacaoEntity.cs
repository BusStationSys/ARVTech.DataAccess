namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ASSOCIACOES")]
    public class AssociacaoEntity
    {
        //[Column("ID")]
        [Description("ID")]     //  DataAnnotation "Description" é usado para casos em que os campos da tabela estão diferentes das propriedades da classe.
        public int Id { get; set; }

        //[Column("DESCRICAO_REGISTRO")]
        [Description("DESCRICAO_REGISTRO")]
        public string DescricaoRegistro { get; set; }

        //[Column("OBSERVACOES")]
        [Description("OBSERVACOES")]
        public string Observacoes { get; set; }

        //[Column("RAZAO_SOCIAL")]
        [Description("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }

        //[Column("SIGLA")]
        [Description("SIGLA")]
        public string Sigla { get; set; }

        public ICollection<CabanhaEntity> Cabanhas { get; set; }

        public override string ToString()
        {
            return $"Associação ID: {this.Id}; Razão Social: {this.RazaoSocial}.";
        }
    }
}