namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PessoaFisicaResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaResponse Pessoa { get; set; }

        public string Cpf { get; set; }

        [NotMapped]
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

        public string Rg { get; set; }

        public DateTime? DataNascimento { get; set; }

        [NotMapped]
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

        public string NumeroCtps { get; set; }

        public string SerieCtps { get; set; }

        public string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física Guid: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}