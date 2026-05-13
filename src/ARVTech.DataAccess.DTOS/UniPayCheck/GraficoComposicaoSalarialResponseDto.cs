namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class GraficoComposicaoSalarialResponseDto
    {
        [Description("GUIDUSUARIO")]
        public Guid GuidUsuario { get; set; }

        [Description("COMPETENCIA")]
        public string Competencia { get; set; }

        [Description("VALOR")]
        public decimal Valor { get; set; }

        [Description("MATRICULA")]
        public string Matricula { get; set; }

        [Description("DESCRICAO_EVENTO")]
        public string DescricaoEvento { get; set; }

        [Description("TIPO")]
        public string Tipo { get; set; }

        [Description("COR")]
        public string Cor { get; set; }

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
    }
}