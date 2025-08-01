namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CABANHAS")]
    public class CabanhaEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("MARCA")]
        public byte[] Marca { get; set; }

        [Description("BAIRRO")]
        public string Bairro { get; set; }

        [Description("CEP")]
        public string Cep { get; set; }

        [Description("CIDADE")]
        public string Cidade { get; set; }

        [Description("CNPJ")]
        public string Cnpj { get; set; }

        [Description("COMPLEMENTO")]
        public string Complemento { get; set; }

        [Description("EMAIL")]
        public string Email { get; set; }

        [Description("ENDERECO")]
        public string Endereco { get; set; }

        [Description("NOME_FANTASIA")]
        public string NomeFantasia { get; set; }

        [Description("NUMERO")]
        public string Numero { get; set; }

        [Description("PONTO_REFERENCIA")]
        public string PontoReferencia { get; set; }

        [Description("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }

        [Description("RESPONSAVEL")]
        public string Responsavel { get; set; }

        [Description("TELEFONE")]
        public string Telefone { get; set; }

        [Description("UF")]
        public string Uf { get; set; }

        [Description("IDASSOCIACAO")]
        public int IdAssociacao { get; set; }

        public AssociacaoEntity Associacao { get; set; }

        [Description("GUIDCONTA")]
        public Guid GuidConta { get; set; }

        public ContaEntity Conta { get; set; }


        public override string ToString()
        {
            return $"Cabanha GUID: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}