namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoCalculoResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaEspelhoPonto { get; set; }

        public MatriculaEspelhoPontoResponse MatriculaEspelhoPonto { get; set; }

        public int IdCalculo { get; set; }

        public CalculoResponse Calculo { get; set; }

        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Cálculo GUID: {this.Guid}.";
        }
    }
}