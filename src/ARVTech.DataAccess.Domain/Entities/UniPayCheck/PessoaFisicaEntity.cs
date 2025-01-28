namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PESSOAS_FISICAS")]
    public class PessoaFisicaEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDPESSOA")]
        public Guid GuidPessoa { get; set; }

        public PessoaEntity Pessoa { get; set; }

        [Description("CPF")]
        public string Cpf { get; set; }

        [Description("RG")]
        public string Rg { get; set; }

        [Description("DATA_NASCIMENTO")]
        public DateTime? DataNascimento { get; set; }

        [Description("NOME")]
        public string Nome { get; set; }

        [Description("ENDERECO")]
        public string Endereco { get; set; }

        [Description("NUMERO")]
        public string Numero { get; set; }

        [Description("BAIRRO")]
        public string Bairro { get; set; }

        [Description("NUMERO_CTPS")]
        public string NumeroCtps { get; set; }

        [Description("SERIE_CTPS")]
        public string SerieCtps { get; set; }

        [Description("UF_CTPS")]
        public string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física GUID: {this.Guid}; Pessoa {this.GuidPessoa}.";
        }
    }
}