namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHOS_PONTO_CALCULOS")]
    public class MatriculaEspelhoPontoCalculoEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDMATRICULA_ESPELHO_PONTO")]
        public Guid GuidMatriculaEspelhoPonto { get; set; }

        [Description("IDCALCULO")]
        public int IdCalculo { get; set; }

        public CalculoEntity Calculo { get; set; }

        [Description("VALOR")]
        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Calculo GUID: {this.Guid}.";
        }
    }
}