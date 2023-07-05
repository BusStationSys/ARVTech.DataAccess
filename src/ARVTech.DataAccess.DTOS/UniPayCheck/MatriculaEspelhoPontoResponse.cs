namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoResponse
    {
        public Guid Guid { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan? HorarioBatida1 { get; set; }

        public TimeSpan? HorarioBatida2 { get; set; }

        public TimeSpan? HorarioBatida3 { get; set; }

        public TimeSpan? HorarioBatida4 { get; set; }

        public TimeSpan? HorarioBatida5 { get; set; }

        public TimeSpan? HorarioBatida6 { get; set; }

        public TimeSpan? HorarioBatida7 { get; set; }

        public TimeSpan? HorarioBatida8 { get; set; }

        public string Competencia { get; set; }

        public Guid GuidMatricula { get; set; }

        public MatriculaResponse Matricula { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}