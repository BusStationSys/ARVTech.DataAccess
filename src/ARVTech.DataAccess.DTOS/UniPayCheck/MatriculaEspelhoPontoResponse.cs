namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Net;

    public class MatriculaEspelhoPontoResponse : ApiResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatricula { get; set; }

        public MatriculaResponse Matricula { get; set; }

        [Display(Name = "Competência")]
        public string Competencia { get; set; }

        public IEnumerable<MatriculaEspelhoPontoCalculoResponse> MatriculaEspelhoPontoCalculos { get; set; }

        public IEnumerable<MatriculaEspelhoPontoMarcacaoResponse> MatriculaEspelhoPontoMarcacoes { get; set; }

        public DateTimeOffset? DataConfirmacao { get; set; }

        public byte[]? IpConfirmacao { get; set; }

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

        [NotMapped]
        public string IpConfirmacaoString
        {
            get
            {
                if (this.IpConfirmacao != null && this.IpConfirmacao.Length > 0)
                {
                    return new IPAddress(
                        this.IpConfirmacao).ToString();
                }

                return string.Empty;
            }
        }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}