namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHOS_PONTO_CALCULOS")]
    public class MatriculaEspelhoPontoCalculoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA_ESPELHO_PONTO")]
        public virtual Guid GuidMatriculaEspelhoPonto { get; set; }

        [Description("IDCALCULO")]
        public virtual int IdCalculo { get; set; }

        public virtual CalculoEntity Calculo { get; set; }

        [Description("VALOR")]
        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Calculo GUID: {this.Guid}.";
        }
    }
}