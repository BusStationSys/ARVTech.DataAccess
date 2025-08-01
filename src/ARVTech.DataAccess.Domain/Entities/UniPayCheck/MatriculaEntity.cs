namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS")]
    public class MatriculaEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("MATRICULA")]
        public string Matricula { get; set; }

        [Description("DATA_ADMISSAO")]
        public DateTime DataAdmissao { get; set; }

        [Description("DATA_DEMISSAO")]
        public DateTime? DataDemissao { get; set; }

        [Description("GUIDCOLABORADOR")]
        public Guid GuidColaborador { get; set; }

        public PessoaFisicaEntity Colaborador { get; set; }

        [Description("GUIDEMPREGADOR")]
        public Guid GuidEmpregador { get; set; }

        public PessoaJuridicaEntity Empregador { get; set; }

        public ICollection<MatriculaDemonstrativoPagamentoEntity> DemonstrativosPagamento { get; set; }

        public ICollection<MatriculaEspelhoPontoEntity> EspelhosPonto { get; set; }

        [Description("AGENCIA")]
        public string Agencia { get; set; }

        [Description("BANCO")]
        public string Banco { get; set; }

        [Description("CONTA")]
        public string Conta { get; set; }

        [Description("DV_CONTA")]
        public string DvConta { get; set; }

        [Description("FORMA_PAGAMENTO")]
        public string FormaPagamento { get; set; }

        [Description("CARGA_HORARIA")]
        public decimal CargaHoraria { get; set; }

        [Description("SALARIO_NOMINAL")]
        public string SalarioNominal { get; set; }

        [Description("DESCRICAO_CARGO")]
        public string DescricaoCargo { get; set; }

        [Description("DESCRICAO_SETOR")]
        public string DescricaoSetor { get; set; }

        [Description("FAIXA_IR")]
        public int FaixaIr { get; set; }

        [Description("FAIXA_SF")]
        public int FaixaSf { get; set; }

        public override string ToString()
        {
            return $"Matrícula GUID: {this.Guid}.";
        }
    }
}