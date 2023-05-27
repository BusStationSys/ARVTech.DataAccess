namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class PessoaFisicaDto
    {
        public Guid? Guid { get; set; }

        public string Cpf { get; set; }

        public string Nome { get; set; }

        public PessoaDto Pessoa { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física Guid: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}