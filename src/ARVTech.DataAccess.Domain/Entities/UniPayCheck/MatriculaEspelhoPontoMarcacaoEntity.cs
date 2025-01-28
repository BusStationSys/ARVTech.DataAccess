namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHOS_PONTO_MARCACOES")]
    public class MatriculaEspelhoPontoMarcacaoEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDMATRICULA_ESPELHO_PONTO")]
        public Guid GuidMatriculaEspelhoPonto { get; set; }

        [Description("DATA")]
        public DateTime Data { get; set; }

        [Description("MARCACAO")]
        public string Marcacao { get; set; }

        [Description("HORAS_EXTRAS_050")]
        public TimeSpan? HorasExtras050 { get; set; }

        [Description("HORAS_EXTRAS_070")]
        public TimeSpan? HorasExtras070 { get; set; }

        [Description("HORAS_EXTRAS_100")]
        public TimeSpan? HorasExtras100 { get; set; }

        [Description("HORAS_CREDITO_BH")]
        public TimeSpan? HorasCreditoBH { get; set; }

        [Description("HORAS_DEBITO_BH")]
        public TimeSpan? HorasDebitoBH { get; set; }

        [Description("HORAS_FALTAS")]
        public TimeSpan? HorasFaltas { get; set; }

        [Description("HORAS_TRABALHADAS")]
        public TimeSpan? HorasTrabalhadas { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Marcação GUID: {this.Guid}.";
        }
    }
}