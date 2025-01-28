namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTAS")]
    public class ContaEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("CNPJ")]
        public string Cnpj { get; set; }

        [Description("NOME_FANTASIA")]
        public string NomeFantasia { get; set; }

        [Description("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }

        [Description("BLOQUEADO")]
        public bool Bloqueado { get; set; }

        [Description("ENDERECO")]
        public string Endereco { get; set; }

        [Description("ENDERECO_COBRANCA")]
        public string EnderecoCobranca { get; set; }

        [Description("NUMERO")]
        public string Numero { get; set; }

        [Description("NUMERO_COBRANCA")]
        public string NumeroCobranca { get; set; }

        [Description("COMPLEMENTO")]
        public string Complemento { get; set; }

        [Description("COMPLEMENTO_COBRANCA")]
        public string ComplementoCobranca { get; set; }

        [Description("PONTO_REFERENCIA")]
        public string PontoReferencia { get; set; }

        [Description("PONTO_REFERENCIA_COBRANCA")]
        public string PontoReferenciaCobranca { get; set; }

        [Description("BAIRRO")]
        public string Bairro { get; set; }

        [Description("BAIRRO_COBRANCA")]
        public string BairroCobranca { get; set; }

        [Description("CEP")]
        public string Cep { get; set; }

        [Description("CEP_COBRANCA")]
        public string CepCobranca { get; set; }

        [Description("CIDADE")]
        public string Cidade { get; set; }

        [Description("CIDADE_COBRANCA")]
        public string CidadeCobranca { get; set; }

        [Description("UF")]
        public string Uf { get; set; }

        [Description("UF_COBRANCA")]
        public string UfCobranca { get; set; }

        [Description("RESPONSAVEL")]
        public string Responsavel { get; set; }

        [Description("RESPONSAVEL_COBRANCA")]
        public string ResponsavelCobranca { get; set; }

        [Description("EMAIL")]
        public string Email { get; set; }

        [Description("EMAIL_COBRANCA")]
        public string EmailCobranca { get; set; }

        [Description("TELEFONE")]
        public string Telefone { get; set; }

        [Description("TELEFONE_COBRANCA")]
        public string TelefoneCobranca { get; set; }

        [Description("DATA_EXPIRACAO")]
        public DateTime? DataExpiracao { get; set; }

        public ICollection<CabanhaEntity> Cabanhas { get; set; }

        public override string ToString()
        {
            return $"Conta GUID: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}