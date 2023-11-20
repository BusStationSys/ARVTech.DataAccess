namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System.ComponentModel.DataAnnotations;

    public class PessoaRequestCreateDto
    {
        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }

        public string Complemento { get; set; }

        public string Endereco { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(75, ErrorMessage = "O E-Mail não pode exceder 75 caracteres.", MinimumLength = 0)]
        public string Email { get; set; }

        public string Numero { get; set; }

        public string Telefone { get; set; }

        public string Uf { get; set; }

        public override string ToString()
        {
            return $"Pessoa: {this.Cidade}-{this.Uf}.";
        }
    }
}