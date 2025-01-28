namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PESSOAS")]
    public class PessoaEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("BAIRRO")]
        public string Bairro { get; set; }

        [Description("CEP")]
        public string Cep { get; set; }

        [Description("CIDADE")]
        public string Cidade { get; set; }

        [Description("COMPLEMENTO")]
        public string Complemento { get; set; }

        [Description("DATA_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [Description("DATA_ULTIMA_ALTERACAO")]
        public DateTime DataUltimaAlteracao { get; set; }

        [Description("EMAIL")]
        public string Email { get; set; }

        [Description("ENDERECO")]
        public string Endereco { get; set; }

        [Description("NUMERO")]
        public string Numero { get; set; }

        [Description("TELEFONE")]
        public string Telefone { get; set; }

        [Description("UF")]
        public string Uf { get; set; }

        public override string ToString()
        {
            return $"Pessoa GUID: {this.Guid}.";
        }
    }
}