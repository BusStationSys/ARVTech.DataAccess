﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class PessoaDto
    {
        public Guid? Guid { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }

        public string Complemento { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Uf { get; set; }

        public override string ToString()
        {
            return $"Pessoa Guid: {this.Guid}.";
        }
    }
}