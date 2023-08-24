namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MatriculaEspelhoPontoResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatricula { get; set; }

        public MatriculaResponse Matricula { get; set; }

        [Display(Name = "Competência")]
        public string Competencia { get; set; }

        public IEnumerable<MatriculaEspelhoPontoCalculoResponse> MatriculaEspelhoPontoCalculos { get; set; }

        public IEnumerable<MatriculaEspelhoPontoMarcacaoResponse> MatriculaEspelhoPontoMarcacoes { get; set; }

        [NotMapped]
        public string CompetenciaFormatada
        {
            get
            {
                DateTime dt = new(
                    Convert.ToInt32(
                        this.Competencia.Substring(0, 4)),
                    Convert.ToInt32(
                        this.Competencia.Substring(4, 2)),
                    1);

                return dt.ToString("MM/yyyy");
            }
        }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}