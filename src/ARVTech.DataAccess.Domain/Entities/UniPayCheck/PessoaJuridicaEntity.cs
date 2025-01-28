namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using ARVTech.DataAccess.Domain.Enums.UniPayCheck;

    [Table("PESSOAS_JURIDICAS")]
    public class PessoaJuridicaEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDPESSOA")]
        public Guid GuidPessoa { get; set; }

        public PessoaEntity Pessoa { get; set; }

        [Description("CNPJ")]
        public string Cnpj { get; set; }

        [Description("DATA_FUNDACAO")]
        public DateTime? DataFundacao { get; set; }

        [Description("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }

        [Description("IDUNIDADE_NEGOCIO")]
        public UnidadeNegocioEnum IdUnidadeNegocio { get; set; }

        public UnidadeNegocioEntity UnidadeNegocio { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica GUID: {this.Guid}; Pessoa {this.GuidPessoa}.";
        }
    }
}