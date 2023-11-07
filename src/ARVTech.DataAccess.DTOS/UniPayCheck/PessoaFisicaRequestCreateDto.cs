﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PessoaFisicaRequestCreateDto
    {
        public PessoaRequestCreateDto Pessoa { get; set; }

        public string Cpf { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento do Nome.")]
        [StringLength(75, ErrorMessage = "O Nome não pode exceder 75 caracteres.")]
        public string Nome { get; set; }

        public string NumeroCtps { get; set; }

        public string SerieCtps { get; set; }

        public string UfCtps { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física Nome: {this.Nome}.";
        }
    }
}