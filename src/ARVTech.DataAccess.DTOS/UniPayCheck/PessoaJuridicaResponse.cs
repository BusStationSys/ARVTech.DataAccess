namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PessoaJuridicaResponse
    {
        private string _cnpj;

        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaResponse Pessoa { get; set; }

        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

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