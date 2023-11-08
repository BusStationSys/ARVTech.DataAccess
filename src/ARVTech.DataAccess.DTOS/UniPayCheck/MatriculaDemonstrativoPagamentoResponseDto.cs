namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Net;

    public class MatriculaDemonstrativoPagamentoResponseDto : ApiResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidMatricula { get; set; }

        public MatriculaResponseDto Matricula { get; set; }

        [Display(Name = "Competência")]
        public string Competencia { get; set; }

        public IEnumerable<MatriculaDemonstrativoPagamentoEventoResponseDto> MatriculaDemonstrativoPagamentoEventos { get; set; }

        public IEnumerable<MatriculaDemonstrativoPagamentoTotalizadorResponseDto> MatriculaDemonstrativoPagamentoTotalizadores { get; set; }

        public DateTimeOffset? DataConfirmacao { get; set; }

        public byte[]? IpConfirmacao { get; set; }

        public string ConteudoArquivo { get; set; }

        [NotMapped]
        public decimal TotalDescontos
        {
            get
            {
                if (MatriculaDemonstrativoPagamentoTotalizadores != null &&
                    MatriculaDemonstrativoPagamentoTotalizadores.Count(
                        t => t.Totalizador.Id == 4) > 0)
                {
                    return MatriculaDemonstrativoPagamentoTotalizadores.Where(
                        t => t.Totalizador.Id == 4).Sum(t => t.Valor);
                }

                return decimal.Zero;
            }
        }

        [NotMapped]
        public decimal TotalLiquido
        {
            get
            {
                if (MatriculaDemonstrativoPagamentoTotalizadores != null &&
                    MatriculaDemonstrativoPagamentoTotalizadores.Count(
                        t => t.Totalizador.Id == 7) > 0)
                {
                    return MatriculaDemonstrativoPagamentoTotalizadores.Where(
                        t => t.Totalizador.Id == 7).Sum(t => t.Valor);
                }

                return decimal.Zero;
            }
        }

        [NotMapped]
        public decimal TotalVencimentos
        {
            get
            {
                if (MatriculaDemonstrativoPagamentoTotalizadores != null &&
                    MatriculaDemonstrativoPagamentoTotalizadores.Count(
                        t => t.Totalizador.Id == 3) > 0)
                {
                    decimal? totalVencimentos = MatriculaDemonstrativoPagamentoTotalizadores.FirstOrDefault(t => t.Totalizador.Id == 3)?.Valor;

                    if (totalVencimentos.HasValue)
                    {
                        return Convert.ToDecimal(
                            totalVencimentos);
                    }
                }

                return decimal.Zero;
            }
        }

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

        [NotMapped]
        public string DataConfirmacaoFormatada
        {
            get
            {
                if (this.DataConfirmacao != null && this.DataConfirmacao.HasValue)
                {
                    return
                        this.DataConfirmacao.Value.AddHours(
                            Convert.ToDouble(
                                this.DataConfirmacao.Value.Offset.TotalHours)).ToString("dd/MM/yyyy HH:mm:ss");
                }

                return string.Empty;
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
            return $"Matrícula Demonstrativo Pagamento GUID: {this.Guid}.";
        }
    }
}