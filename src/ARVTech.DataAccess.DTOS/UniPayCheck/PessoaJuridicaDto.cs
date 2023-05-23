namespace ARVTech.DataAccess.DTOs.UniPayCheck
{ 
    using System;

    public class PessoaJuridicaDto
    {
        public Guid? Guid { get; set; }

        public string RazaoSocial { get; set; }

        public PessoaDto Pessoa { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica Guid: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}