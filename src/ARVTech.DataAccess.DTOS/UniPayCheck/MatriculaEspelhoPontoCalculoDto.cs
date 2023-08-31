namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoCalculoDto
    {
        public Guid? Guid { get; set; }

        public Guid GuidMatriculaEspelhoPonto { get; set; }

        public int IdCalculo { get; set; }

        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Cálculo GUID: {this.Guid}.";
        }
    }
}