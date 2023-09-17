﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class PessoaJuridicaRequestDto
    {
        public Guid Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaRequestDto Pessoa { get; set; }

        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

        public string RazaoSocial { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica Guid: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}