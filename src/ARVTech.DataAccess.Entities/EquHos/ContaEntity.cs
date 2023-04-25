namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTAS")]
    public class ContaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("CNPJ")]
        public virtual string Cnpj { get; set; }

        [Description("NOME_FANTASIA")]
        public virtual string NomeFantasia { get; set; }

        [Description("RAZAO_SOCIAL")]
        public virtual string RazaoSocial { get; set; }

        [Description("BLOQUEADO")]
        public virtual bool Bloqueado { get; set; }

        [Description("ENDERECO")]
        public virtual string Endereco { get; set; }

        [Description("ENDERECO_COBRANCA")]
        public virtual string EnderecoCobranca { get; set; }

        [Description("NUMERO")]
        public virtual string Numero { get; set; }

        [Description("NUMERO_COBRANCA")]
        public virtual string NumeroCobranca { get; set; }

        [Description("COMPLEMENTO")]
        public virtual string Complemento { get; set; }

        [Description("COMPLEMENTO_COBRANCA")]
        public virtual string ComplementoCobranca { get; set; }

        [Description("PONTO_REFERENCIA")]
        public virtual string PontoReferencia { get; set; }

        [Description("PONTO_REFERENCIA_COBRANCA")]
        public virtual string PontoReferenciaCobranca { get; set; }

        [Description("BAIRRO")]
        public virtual string Bairro { get; set; }

        [Description("BAIRRO_COBRANCA")]
        public virtual string BairroCobranca { get; set; }

        [Description("CEP")]
        public virtual string Cep { get; set; }

        [Description("CEP_COBRANCA")]
        public virtual string CepCobranca { get; set; }

        [Description("CIDADE")]
        public virtual string Cidade { get; set; }

        [Description("CIDADE_COBRANCA")]
        public virtual string CidadeCobranca { get; set; }

        [Description("UF")]
        public virtual string Uf { get; set; }

        [Description("UF_COBRANCA")]
        public virtual string UfCobranca { get; set; }

        [Description("RESPONSAVEL")]
        public virtual string Responsavel { get; set; }

        [Description("RESPONSAVEL_COBRANCA")]
        public virtual string ResponsavelCobranca { get; set; }

        [Description("EMAIL")]
        public virtual string Email { get; set; }

        [Description("EMAIL_COBRANCA")]
        public virtual string EmailCobranca { get; set; }

        [Description("TELEFONE")]
        public virtual string Telefone { get; set; }

        [Description("TELEFONE_COBRANCA")]
        public virtual string TelefoneCobranca { get; set; }

        [Description("DATA_EXPIRACAO")]
        public virtual DateTime? DataExpiracao { get; set; }

        public virtual ICollection<CabanhaEntity> Cabanhas { get; set; }

        public override string ToString()
        {
            return $"Conta GUID: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}