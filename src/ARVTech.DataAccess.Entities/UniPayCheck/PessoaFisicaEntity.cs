namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PESSOAS_FISICAS")]
    public class PessoaFisicaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDPESSOA")]
        public virtual Guid GuidPessoa { get; set; }

        public virtual PessoaEntity Pessoa { get; set; }

        [Description("CPF")]
        public virtual string Cpf { get; set; }

        [Description("RG")]
        public virtual string Rg { get; set; }

        [Description("DATA_NASCIMENTO")]
        public virtual DateTime? DataNascimento { get; set; }

        [Description("NOME")]
        public virtual string Nome { get; set; }

        [Description("NUMERO_CTPS")]
        public virtual string NumeroCtps { get; set; }

        [Description("SERIE_CTPS")]
        public virtual string SerieCtps { get; set; }

        [Description("UF_CTPS")]
        public virtual string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física GUID: {this.Guid}; Pessoa {this.GuidPessoa}.";
        }
    }
}