namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHOS_PONTO_MARCACOES")]
    public class MatriculaEspelhoPontoMarcacaoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA_ESPELHO_PONTO")]
        public virtual Guid GuidMatriculaEspelhoPonto { get; set; }

        [Description("DATA")]
        public virtual DateTime Data { get; set; }

        [Description("MARCACAO")]
        public virtual string Marcacao { get; set; }

        [Description("HORAS_EXTRAS_050")]
        public virtual TimeSpan? HorasExtras050 { get; set; }

        [Description("HORAS_EXTRAS_070")]
        public virtual TimeSpan? HorasExtras070 { get; set; }

        [Description("HORAS_EXTRAS_100")]
        public virtual TimeSpan? HorasExtras100 { get; set; }

        [Description("HORAS_CREDITO_BH")]
        public virtual TimeSpan? HorasCreditoBH { get; set; }

        [Description("HORAS_DEBITO_BH")]
        public virtual TimeSpan? HorasDebitoBH { get; set; }

        [Description("HORAS_FALTAS")]
        public virtual TimeSpan? HorasFaltas { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Marcação GUID: {this.Guid}.";
        }
    }
}