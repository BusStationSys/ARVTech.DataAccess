namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using ARVTech.DataAccess.Enums;

    [Table("PESSOAS_JURIDICAS")]
    public class PessoaJuridicaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDPESSOA")]
        public virtual Guid GuidPessoa { get; set; }

        public virtual PessoaEntity Pessoa { get; set; }

        [Description("CNPJ")]
        public virtual string Cnpj { get; set; }

        [Description("DATA_FUNDACAO")]
        public virtual DateTime? DataFundacao { get; set; }

        [Description("RAZAO_SOCIAL")]
        public virtual string RazaoSocial { get; set; }

        [Description("IDUNIDADE_NEGOCIO")]
        public virtual UnidadeNegocioEnum IdUnidadeNegocio { get; set; }

        public virtual UnidadeNegocioEntity UnidadeNegocio { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica GUID: {this.Guid}; Pessoa {this.GuidPessoa}.";
        }
    }
}