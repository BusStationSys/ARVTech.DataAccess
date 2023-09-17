namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ARVTech.Shared;

    public class MatriculaResponseDto
    {
        public Guid Guid { get; set; }

        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        public DateTime DataAdmissao { get; set; }

        public DateTime? DataDemissao { get; set; }

        public string DescricaoCargo { get; set; }

        public string DescricaoSetor { get; set; }

        public Guid GuidColaborador { get; set; }

        public PessoaFisicaResponseDto Colaborador { get; set; }

        public Guid GuidEmpregador { get; set; }

        public PessoaJuridicaResponseDto Empregador { get; set; }

        public IEnumerable<MatriculaDemonstrativoPagamentoResponseDto>? DemonstrativosPagamento { get; set; }

        public IEnumerable<MatriculaEspelhoPontoResponseDto>? EspelhosPonto { get; set; }

        public string Banco { get; set; }

        public string Agencia { get; set; }

        public string Conta { get; set; }

        public decimal CargaHoraria { get; set; }

        public string SalarioNominal { get; set; }

        public int FaixaIr { get; set; }

        public int FaixaSf { get; set; }

        [NotMapped]
        public decimal SalarioNominalDescriptografado
        {
            get
            {
                //  Atualiza o Salário Nominal criptografando a informação usando como chave o GuidMatricula.
                var key = this.Guid.ToString("N").ToUpper();

                string normalValue = PasswordCryptography.DecryptString(
                    key,
                    this.SalarioNominal);

                if (!string.IsNullOrEmpty(
                    normalValue))
                    return Convert.ToDecimal(
                        normalValue);

                return 0.01M;
            }
        }

        [NotMapped]
        public string SalarioNominalFormatado
        {
            get
            {
                return this.SalarioNominalDescriptografado.ToString("#,###,###,##0.00");
            }
        }

        public override string ToString()
        {
            return $"Matrícula GUID: {this.Guid}.";
        }
    }
}