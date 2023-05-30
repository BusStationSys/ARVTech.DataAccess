namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHO_PONTO")]
    public class MatriculaEspelhoPontoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("DATA")]
        public virtual DateTime Data { get; set; }

        [Description("HORARIO_BATIDA_1")]
        public virtual TimeSpan? HorarioBatida1 { get; set; }

        [Description("HORARIO_BATIDA_2")]
        public virtual TimeSpan? HorarioBatida2 { get; set; }

        [Description("HORARIO_BATIDA_3")]
        public virtual TimeSpan? HorarioBatida3 { get; set; }

        [Description("HORARIO_BATIDA_4")]
        public virtual TimeSpan? HorarioBatida4 { get; set; }

        [Description("HORARIO_BATIDA_5")]
        public virtual TimeSpan? HorarioBatida5 { get; set; }

        [Description("HORARIO_BATIDA_6")]
        public virtual TimeSpan? HorarioBatida6 { get; set; }

        [Description("HORARIO_BATIDA_7")]
        public virtual TimeSpan? HorarioBatida7 { get; set; }

        [Description("HORARIO_BATIDA_8")]
        public virtual TimeSpan? HorarioBatida8 { get; set; }

        [Description("COMPETENCIA")]
        public virtual string Competencia { get; set; }

        [Description("GUIDMATRICULA")]
        public virtual Guid GuidMatricula { get; set; }

        public virtual MatriculaEntity Matricula { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}