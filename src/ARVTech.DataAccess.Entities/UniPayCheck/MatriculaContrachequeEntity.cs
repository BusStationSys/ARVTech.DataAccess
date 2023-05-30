﻿namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_CONTRACHEQUE")]
    public class MatriculaContrachequeEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA")]
        public virtual Guid GuidMatricula { get; set; }

        public virtual MatriculaEntity Matricula { get; set; }

        [Description("COMPETENCIA")]
        public virtual string Competencia { get; set; }

        public override string ToString()
        {
            return $"Matrícula Contracheque GUID: {this.Guid}.";
        }
    }
}