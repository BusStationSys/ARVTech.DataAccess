namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoCalculoResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaEspelhoPonto { get; set; }

        public MatriculaEspelhoPontoResponseDto MatriculaEspelhoPonto { get; set; }

        public int IdCalculo { get; set; }

        public CalculoResponseDto Calculo { get; set; }

        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Cálculo GUID: {this.Guid}.";
        }
    }
}