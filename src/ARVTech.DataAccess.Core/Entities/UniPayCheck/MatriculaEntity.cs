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

        public override string ToString()
        {
            return $"Matrícula GUID: {this.Guid}.";
        }
    }
}