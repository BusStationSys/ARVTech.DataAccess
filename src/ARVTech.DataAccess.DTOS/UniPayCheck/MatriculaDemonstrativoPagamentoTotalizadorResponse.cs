namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaDemonstrativoPagamentoTotalizadorResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponse MatriculaDemonstrativoPagamento { get; set; }

        public int IdTotalizador { get; set; }

        public TotalizadorResponse Totalizador { get; set; }

        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Totalizador GUID: {this.Guid}.";
        }
    }
}