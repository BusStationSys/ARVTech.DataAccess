﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using ARVTech.Shared.Extensions;

    public class MatriculaEspelhoPontoMarcacaoResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaEspelhoPonto { get; set; }

        public MatriculaEspelhoPontoResponseDto MatriculaEspelhoPonto { get; set; }

        public DateTime Data { get; set; }

        public string Marcacao { get; set; }

        public TimeSpan? HorasCreditoBH { get; set; }

        public TimeSpan? HorasDebitoBH { get; set; }

        public TimeSpan? HorasExtras050 { get; set; }

        public TimeSpan? HorasExtras070 { get; set; }

        public TimeSpan? HorasExtras100 { get; set; }

        public TimeSpan? HorasFaltas { get; set; }

        public TimeSpan? HorasTrabalhadas { get; set; }

        [NotMapped]
        public string DiaSemana
        {
            get
            {
                return this.Data.ToString(
                    "dddd",
                    new CultureInfo("pt-BR")).FirstCharToUpper();
            }
        }

        [NotMapped]
        public string HorasCreditoBHFormatada
        {
            get
            {
                if (this.HorasCreditoBH != null &&
                    this.HorasCreditoBH.HasValue)
                    return string.Concat(
                        this.HorasCreditoBH.Value.Hours.ToString("00"),
                        ":",
                        this.HorasCreditoBH.Value.Minutes.ToString("00"));

                return "00:00";
            }
        }

        [NotMapped]
        public string HorasCreditoBHString
        {
            get
            {
                return string.IsNullOrEmpty(this.HorasCreditoBHFormatada) ||
                    this.HorasCreditoBHFormatada == "00:00" ?
                    "--" :
                    this.HorasCreditoBHFormatada;
            }
        }

        [NotMapped]
        public string HorasDebitoBHFormatada
        {
            get
            {
                if (this.HorasDebitoBH != null &&
                    this.HorasDebitoBH.HasValue)
                    return string.Concat(
                        this.HorasDebitoBH.Value.Hours.ToString("00"),
                        ":",
                        this.HorasDebitoBH.Value.Minutes.ToString("00"));

                return "00:00";
            }
        }

        [NotMapped]
        public string HorasDebitoBHString
        {
            get
            {
                return string.IsNullOrEmpty(this.HorasDebitoBHFormatada) ||
                    this.HorasDebitoBHFormatada == "00:00" ?
                    "--" :
                    this.HorasDebitoBHFormatada;
            }
        }

        [NotMapped]
        public string HorasExtras050Formatada
        {
            get
            {
                if (this.HorasExtras050 != null &&
                    this.HorasExtras050.HasValue)
                    return string.Concat(
                        this.HorasExtras050.Value.Hours.ToString("00"),
                        ":",
                        this.HorasExtras050.Value.Minutes.ToString("00"));

                return "00:00";
            }
        }

        [NotMapped]
        public string HorasExtras050String
        {
            get
            {
                return string.IsNullOrEmpty(this.HorasExtras050Formatada) ||
                    this.HorasExtras050Formatada == "00:00" ?
                    "--" :
                    this.HorasExtras050Formatada;
            }
        }

        [NotMapped]
        public string HorasExtras070Formatada
        {
            get
            {
                if (this.HorasExtras070 != null &&
                    this.HorasExtras070.HasValue)
                    return string.Concat(
                        this.HorasExtras070.Value.Hours.ToString("00"),
                        ":",
                        this.HorasExtras070.Value.Minutes.ToString("00"));

                return "00:00";
            }
        }

        [NotMapped]
        public string HorasExtras070String
        {
            get
            {
                return string.IsNullOrEmpty(this.HorasExtras070Formatada) ||
                    this.HorasExtras070Formatada == "00:00" ?
                    "--" :
                    this.HorasExtras070Formatada;
            }
        }

        [NotMapped]
        public string HorasExtras100Formatada
        {
            get
            {
                if (this.HorasExtras100 != null &&
                    this.HorasExtras100.HasValue)
                    return string.Concat(
                        this.HorasExtras100.Value.Hours.ToString("00"),
                        ":",
                        this.HorasExtras100.Value.Minutes.ToString("00"));

                return "00:00";
            }
        }

        [NotMapped]
        public string HorasExtras100String
        {
            get
            {
                return string.IsNullOrEmpty(this.HorasExtras100Formatada) ||
                    this.HorasExtras100Formatada == "00:00" ?
                    "--" :
                    this.HorasExtras100Formatada;
            }
        }

        [NotMapped]
        public string HorasFaltasFormatada
        {
            get
            {
                if (this.HorasFaltas != null &&
                    this.HorasFaltas.HasValue)
                    return string.Concat(
                        this.HorasFaltas.Value.Hours.ToString("00"),
                        ":",
                        this.HorasFaltas.Value.Minutes.ToString("00"));

                return "00:00";
            }
        }

        [NotMapped]
        public string HorasFaltasString
        {
            get
            {
                return string.IsNullOrEmpty(this.HorasFaltasFormatada) ||
                    this.HorasFaltasFormatada == "00:00" ?
                    "--" :
                    this.HorasFaltasFormatada;
            }
        }

        [NotMapped]
        public string HorasTrabalhadasFormatada
        {
            get
            {
                if (this.HorasTrabalhadas != null &&
                    this.HorasTrabalhadas.HasValue)
                    return string.Concat(
                        this.HorasTrabalhadas.Value.Hours.ToString("00"),
                        ":",
                        this.HorasTrabalhadas.Value.Minutes.ToString("00"));

                return "00:00";
            }
        }

        [NotMapped]
        public string HorasTrabalhadasString
        {
            get
            {
                return string.IsNullOrEmpty(this.HorasTrabalhadasFormatada) ||
                    this.HorasTrabalhadasFormatada == "00:00" ?
                    "--" :
                    this.HorasTrabalhadasFormatada;
            }
        }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Marcação GUID: {this.Guid}.";
        }
    }
}