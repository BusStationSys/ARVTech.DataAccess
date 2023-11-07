namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PessoaJuridicaRequestUpdateDto
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento da Razão Social.")]
        [StringLength(75, ErrorMessage = "A Razão Social não pode exceder 75 caracteres.")]
        public string RazaoSocial { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica Guid: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}