namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PessoaFisicaResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaResponse Pessoa { get; set; }

        [DisplayFormat(DataFormatString = "###.###.###-##")]
        public string Cpf { get; set; }

        public string Rg { get; set; }

        public DateTime? DataNascimento { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        public string NumeroCtps { get; set; }

        public string SerieCtps { get; set; }

        public string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física Guid: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}