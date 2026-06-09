namespace ARVTech.DataAccess.Contracts.PayCheck.Requests
{
    using System;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;

    public class MatriculaDemonstrativoPagamentoTotalizadorRequest
    {
        public Guid? Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponse MatriculaDemonstrativoPagamento { get; set; }

        public int IdTotalizador { get; set; }

        public TotalizadorRequest Totalizador { get; set; }

        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Totalizador GUID: {this.Guid}.";
        }
    }
}