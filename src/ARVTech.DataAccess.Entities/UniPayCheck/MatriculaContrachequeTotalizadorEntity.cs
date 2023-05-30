namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_CONTRACHEQUE_TOTALIZADORES")]
    public class MatriculaContrachequeTotalizadorEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA_CONTRACHEQUE")]
        public virtual Guid GuidMatriculaContracheque { get; set; }

        public virtual MatriculaContrachequeEntity MatriculaContracheque { get; set; }

        [Description("IDTOTALIZADOR")]
        public virtual int IdTotalizador { get; set; }

        public virtual TotalizadorEntity Totalizador { get; set; }

        [Description("VALOR")]
        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Contracheque Totalizador GUID: {this.Guid}.";
        }
    }
}