namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PessoaJuridicaResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaResponseDto Pessoa { get; set; }

        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        [NotMapped]
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