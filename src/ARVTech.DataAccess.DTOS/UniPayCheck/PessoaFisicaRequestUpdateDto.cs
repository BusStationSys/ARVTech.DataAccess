﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PessoaFisicaRequestUpdateDto
    {
        [Required(ErrorMessage = "É necessário o preenchimento do GUID.")]
        public Guid? Guid { get; set; }

        public Guid GuidPessoa { get; set; }

        public PessoaRequestUpdateDto Pessoa { get; set; }

        public string Cpf { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "É necessário o preenchimento da Data de Nascimento.")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento do Nome.")]
        [StringLength(100, ErrorMessage = "O Nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        public string NumeroCtps { get; set; }

        [StringLength(20, ErrorMessage = "O RG não pode exceder 20 caracteres.", MinimumLength = 0)]
        public string? Rg { get; set; }

        public string SerieCtps { get; set; }

        public string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física Guid: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}