namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES")]
    public class MatriculaDemonstrativoPagamentoTotalizadorEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO")]
        public virtual Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public virtual MatriculaDemonstrativoPagamentoEntity MatriculaDemonstrativoPagamento { get; set; }

        [Description("IDTOTALIZADOR")]
        public virtual int IdTotalizador { get; set; }

        public virtual TotalizadorEntity Totalizador { get; set; }

        [Description("VALOR")]
        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Totalizador GUID: {this.Guid}.";
        }
    }
}