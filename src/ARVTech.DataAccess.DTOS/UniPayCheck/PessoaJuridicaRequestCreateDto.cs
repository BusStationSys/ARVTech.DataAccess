﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using ARVTech.DataAccess.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PessoaJuridicaRequestCreateDto
    {
        public Guid GuidPessoa { get; set; }

        public PessoaRequestCreateDto Pessoa { get; set; }

        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento da Razão Social.")]
        [StringLength(75, ErrorMessage = "A Razão Social não pode exceder 75 caracteres.")]
        public string RazaoSocial { get; set; }

        public UnidadeNegocioEnum IdBandeiraComercial { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica Razão Social: {this.RazaoSocial}.";
        }
    }
}