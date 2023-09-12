﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaEspelhoPontoRequestCreateDto
    {
        public Guid? Guid { get; set; }

        public Guid? GuidMatricula { get; set; }

        public string Competencia { get; set; }

        public DateTime? DataConfirmacao { get; set; }

        public byte[]? IpConfirmacao { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}