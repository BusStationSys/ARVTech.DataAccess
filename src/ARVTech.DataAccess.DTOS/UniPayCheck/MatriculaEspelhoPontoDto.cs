namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoDto
    {
        public Guid? Guid { get; set; }

        public Guid? GuidMatricula { get; set; }

        public string Competencia { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}