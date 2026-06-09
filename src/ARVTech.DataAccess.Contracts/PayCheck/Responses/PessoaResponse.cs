namespace ARVTech.DataAccess.Contracts.PayCheck.Responses
{
    using System;

    public record PessoaResponse
    {
        public Guid Guid { get; set; }

        public string? Bairro { get; set; }

        public string? Cep { get; set; }

        public required string Cidade { get; set; }

        public string? Complemento { get; set; }

        public string? Email { get; set; }

        public required string Endereco { get; set; }

        public string? Numero { get; set; }

        public string? Telefone { get; set; }

        public required string Uf { get; set; }

        public override string ToString()
        {
            return $"Pessoa GUID: {this.Guid}.";
        }
    }
}