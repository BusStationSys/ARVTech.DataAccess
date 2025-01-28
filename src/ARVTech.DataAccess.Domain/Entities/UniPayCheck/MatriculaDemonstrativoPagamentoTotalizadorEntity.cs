namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES")]
    public class MatriculaDemonstrativoPagamentoTotalizadorEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO")]
        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        [Description("IDTOTALIZADOR")]
        public int IdTotalizador { get; set; }

        public TotalizadorEntity Totalizador { get; set; }

        [Description("VALOR")]
        public string Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Totalizador GUID: {this.Guid}.";
        }
    }
}