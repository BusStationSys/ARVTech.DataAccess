namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ARVTech.DataAccess.Enums;

    public class PessoaJuridicaResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaResponseDto Pessoa { get; set; }

        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

        [NotMapped]
        [Display(Name = "Fundação")]
        public string DataFundacaoFormatada
        {
            get
            {
                if (this.DataFundacao != null &&
                    this.DataFundacao.HasValue)
                {
                    return Convert.ToDateTime(
                        this.DataFundacao).ToString("dd/MM/yyyy");
                }

                return "__/__/____";
            }
        }

        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        public UnidadeNegocioEnum IdUnidadeNegocio { get; set; }

        public UnidadeNegocioResponseDto UnidadeNegocio { get; set; }

        [NotMapped]
        [Display(Name = "CNPJ")]
        public string CnpjFormatado
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Cnpj))
                {
                    return Convert.ToInt64(
                        this.Cnpj).ToString(
                            @"00\.000\.000\/0000\-00");
                }

                return @"00\.000\.000\/0000\-00";
            }
        }

        public override string ToString()
        {
            return $"Pessoa Jurídica Guid: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}