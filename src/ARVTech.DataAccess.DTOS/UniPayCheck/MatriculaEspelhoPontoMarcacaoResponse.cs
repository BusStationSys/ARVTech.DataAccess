namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoMarcacaoResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaEspelhoPonto { get; set; }

        public MatriculaEspelhoPontoResponse MatriculaEspelhoPonto { get; set; }

        public DateTime Data { get; set; }

        public string Marcacao { get; set; }

        public TimeSpan? HorasExtras050 { get; set; }

        public TimeSpan? HorasExtras070 { get; set; }

        public TimeSpan? HorasExtras100 { get; set; }

        public TimeSpan? HorasCreditoBH { get; set; }

        public TimeSpan? HorasDebitoBH { get; set; }

        public TimeSpan? HorasFaltas { get; set; }

        public TimeSpan? HorasTrabalhadas { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Marcação GUID: {this.Guid}.";
        }
    }
}