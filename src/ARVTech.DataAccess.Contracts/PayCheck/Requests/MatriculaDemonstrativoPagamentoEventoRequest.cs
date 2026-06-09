namespace ARVTech.DataAccess.Contracts.PayCheck.Requests
{
    using System;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;

    public class MatriculaDemonstrativoPagamentoEventoRequest
    {
        public Guid? Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponse MatriculaDemonstrativoPagamento { get; set; }

        public int IdEvento { get; set; }

        public EventoRequest Evento { get; set; }

        public decimal? Referencia { get; set; }

        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}