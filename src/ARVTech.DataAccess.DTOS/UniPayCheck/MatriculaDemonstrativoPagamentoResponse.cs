namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Linq;

    public class MatriculaDemonstrativoPagamentoResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatricula { get; set; }

        [Display(Name = "Matrícula")]
        public MatriculaResponse Matricula { get; set; }

        [Display(Name = "Competência")]
        public string Competencia { get; set; }

        public IEnumerable<MatriculaDemonstrativoPagamentoEventoResponse> MatriculaDemonstrativoPagamentoEventos { get; set; }

        public IEnumerable<MatriculaDemonstrativoPagamentoTotalizadorResponse> MatriculaDemonstrativoPagamentoTotalizadores { get; set; }

        [NotMapped]
        public string AnoCompetencia
        {
            get
            {
                return this.Competencia.Substring(0, 4);
            }
        }

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
                    Convert.ToInt32(
                        this.Competencia.Substring(6, 2)));

                return dt.ToString("MM/yyyy");
            }
        }

        [NotMapped]
        public string MesCompetencia
        {
            get
            {
                return this.Competencia.Substring(4, 2);
            }
        }

        [NotMapped]
        public int OrdemCompetencia
        {
            get
            {
                return Convert.ToInt32(
                    this.Competencia.Substring(6, 2));

            }
        }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento GUID: {this.Guid}.";
        }
    }
}