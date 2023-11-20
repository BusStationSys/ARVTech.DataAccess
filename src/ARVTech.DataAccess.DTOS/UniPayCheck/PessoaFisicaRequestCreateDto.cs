namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PessoaFisicaRequestCreateDto
    {
        public PessoaRequestCreateDto Pessoa { get; set; }

        public string Cpf { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "É necessário o preenchimento da Data de Nascimento.")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento do Nome.")]
        [StringLength(100, ErrorMessage = "O Nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        public string NumeroCtps { get; set; }

        public string SerieCtps { get; set; }

        public string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física Nome: {this.Nome}.";
        }
    }
}