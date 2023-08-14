﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MatriculaResponse
    {
        public Guid Guid { get; set; }

        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        public DateTime DataAdmissao { get; set; }

        public DateTime? DataDemissao { get; set; }

        public Guid GuidColaborador { get; set; }

        public PessoaFisicaResponse Colaborador { get; set; }

        public Guid GuidEmpregador { get; set; }

        public PessoaJuridicaResponse Empregador { get; set; }

        public IEnumerable<MatriculaDemonstrativoPagamentoResponse>? DemonstrativosPagamento { get; set; }

        public IEnumerable<MatriculaEspelhoPontoResponse>? EspelhosPonto { get; set; }

        public string Banco { get; set; }

        public string Agencia { get; set; }

        public string Conta { get; set; }

        public decimal SalarioNominal { get; set; }

        [NotMapped]
        public string SalarioNominalFormatado
        {
            get
            {
                return this.SalarioNominal.ToString("#,###,###,##0.00");
            }
        }

        public override string ToString()
        {
            return $"Matrícula GUID: {this.Guid}.";
        }
    }
}