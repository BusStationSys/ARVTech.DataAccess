﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ARVTech.DataAccess.Domain.Enums.UniPayCheck;

    public class PessoaJuridicaRequestUpdateDto
    {
        [Required(ErrorMessage = "É necessário o preenchimento do GUID.")]
        public Guid? Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaRequestUpdateDto Pessoa { get; set; }

        public string Cnpj { get; set; }

        public DateTime? DataFundacao { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento da Razão Social.")]
        [StringLength(75, ErrorMessage = "A Razão Social não pode exceder 75 caracteres.")]
        public string RazaoSocial { get; set; }

        public UnidadeNegocioEnum IdUnidadeNegocio { get; set; }

        public override string ToString()
        {
            return $"Pessoa Jurídica Guid: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}