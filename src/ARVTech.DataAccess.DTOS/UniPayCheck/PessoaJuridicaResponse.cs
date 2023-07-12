namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PessoaJuridicaResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaResponse Pessoa { get; set; }

        [DisplayFormat(DataFormatString = "##.###.###/####-##")]
        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

        public string RazaoSocial { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica Guid: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}