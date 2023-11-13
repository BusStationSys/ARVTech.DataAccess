namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PessoaFisicaResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaResponseDto Pessoa { get; set; }

        public string Cpf { get; set; }

        [NotMapped]
        [Display(Name = "CPF")]
        public string CpfFormatado
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Cpf))
                {
                    return Convert.ToInt64(
                        this.Cpf).ToString(
                            @"000\.000\.0000\-00");
                }

                return @"000\.000\.0000\-00";
            }
        }

        [Display(Name = "RG")]
        public string Rg { get; set; }

        public DateTime? DataNascimento { get; set; }

        [NotMapped]
        [Display(Name = "Nascimento")]
        public string DataNascimentoFormatada
        {
            get
            {
                if (this.DataNascimento != null &&
                    this.DataNascimento.HasValue)
                {
                    return Convert.ToDateTime(
                        this.DataNascimento).ToString("dd/MM/yyyy");
                }

                return "__/__/____";
            }
        }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        public string NumeroCtps { get; set; }

        public string SerieCtps { get; set; }

        public string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física Guid: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}