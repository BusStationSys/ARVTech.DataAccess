namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatricula { get; set; }

        public MatriculaResponse Matricula { get; set; }

        public string Competencia { get; set; }

        //public IEnumerable<MatriculaEspelhoPontoCalculoEntity> MatriculaEspelhoPontoCalculos { get; set; }

        //public IEnumerable<MatriculaEspelhoPontoMarcacaoEntity> MatriculaEspelhoPontoMarcacoes { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}