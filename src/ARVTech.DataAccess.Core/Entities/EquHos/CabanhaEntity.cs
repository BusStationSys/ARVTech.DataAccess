namespace ARVTech.DataAccess.Core.Entities.EquHos
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CABANHAS")]
    public class CabanhaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("MARCA")]
        public virtual byte[] Marca { get; set; }

        [Description("BAIRRO")]
        public virtual string Bairro { get; set; }

        [Description("CEP")]
        public virtual string Cep { get; set; }

        [Description("CIDADE")]
        public virtual string Cidade { get; set; }

        [Description("CNPJ")]
        public virtual string Cnpj { get; set; }

        [Description("COMPLEMENTO")]
        public virtual string Complemento { get; set; }

        [Description("EMAIL")]
        public virtual string Email { get; set; }

        [Description("ENDERECO")]
        public virtual string Endereco { get; set; }

        [Description("NOME_FANTASIA")]
        public virtual string NomeFantasia { get; set; }

        [Description("NUMERO")]
        public virtual string Numero { get; set; }

        [Description("PONTO_REFERENCIA")]
        public virtual string PontoReferencia { get; set; }

        [Description("RAZAO_SOCIAL")]
        public virtual string RazaoSocial { get; set; }

        [Description("RESPONSAVEL")]
        public virtual string Responsavel { get; set; }

        [Description("TELEFONE")]
        public virtual string Telefone { get; set; }

        [Description("UF")]
        public virtual string Uf { get; set; }

        [Description("IDASSOCIACAO")]
        public virtual int IdAssociacao { get; set; }

        public virtual AssociacaoEntity Associacao { get; set; }

        [Description("GUIDCONTA")]
        public virtual Guid GuidConta { get; set; }

        public virtual ContaEntity Conta { get; set; }


        public override string ToString()
        {
            return $"Cabanha GUID: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}