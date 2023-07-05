namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MatriculaDemonstrativoPagamentoEventoResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponse MatriculaDemonstrativoPagamento { get; set; }

        public int IdEvento { get; set; }

        public EventoResponse Evento { get; set; }

        public decimal? Referencia { get; set; }

        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}