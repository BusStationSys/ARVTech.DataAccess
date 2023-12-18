namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS")]
    public class MatriculaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("MATRICULA")]
        public virtual string Matricula { get; set; }

        [Description("DATA_ADMISSAO")]
        public virtual DateTime DataAdmissao { get; set; }

        [Description("DATA_DEMISSAO")]
        public virtual DateTime? DataDemissao { get; set; }

        [Description("GUIDCOLABORADOR")]
        public virtual Guid GuidColaborador { get; set; }

        public virtual PessoaFisicaEntity Colaborador { get; set; }

        [Description("GUIDEMPREGADOR")]
        public virtual Guid GuidEmpregador { get; set; }

        public virtual PessoaJuridicaEntity Empregador { get; set; }

        public virtual ICollection<MatriculaDemonstrativoPagamentoEntity> DemonstrativosPagamento { get; set; }

        public virtual ICollection<MatriculaEspelhoPontoEntity> EspelhosPonto { get; set; }

        [Description("AGENCIA")]
        public virtual string Agencia { get; set; }

        [Description("BANCO")]
        public virtual string Banco { get; set; }

        [Description("CONTA")]
        public virtual string Conta { get; set; }

        [Description("DV_CONTA")]
        public virtual string DvConta { get; set; }

        [Description("FORMA_PAGAMENTO")]
        public virtual string FormaPagamento { get; set; }

        [Description("CARGA_HORARIA")]
        public virtual decimal CargaHoraria { get; set; }

        [Description("SALARIO_NOMINAL")]
        public virtual string SalarioNominal { get; set; }

        [Description("DESCRICAO_CARGO")]
        public virtual string DescricaoCargo { get; set; }

        [Description("DESCRICAO_SETOR")]
        public virtual string DescricaoSetor { get; set; }

        [Description("FAIXA_IR")]
        public virtual int FaixaIr { get; set; }

        [Description("FAIXA_SF")]
        public virtual int FaixaSf { get; set; }

        public override string ToString()
        {
            return $"Matrícula GUID: {this.Guid}.";
        }
    }
}