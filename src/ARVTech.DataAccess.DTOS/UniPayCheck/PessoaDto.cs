namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class PessoaDto
    {
        public Guid? Guid { get; set; }

        public override string ToString()
        {
            return $"Pessoa Guid: {this.Guid}.";
        }
    }
}