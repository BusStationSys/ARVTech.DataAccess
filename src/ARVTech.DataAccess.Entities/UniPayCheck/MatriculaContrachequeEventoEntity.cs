namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_CONTRACHEQUE_EVENTOS")]
    public class MatriculaContrachequeEventoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA_CONTRACHEQUE")]
        public virtual Guid GuidMatriculaContracheque { get; set; }

        public virtual MatriculaContrachequeEntity MatriculaContracheque { get; set; }

        [Description("IDEVENTO")]
        public virtual int IdEvento { get; set; }

        public virtual EventoEntity Evento { get; set; }

        [Description("VALOR")]
        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Contracheque Evento GUID: {this.Guid}.";
        }
    }
}