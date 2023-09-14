namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaDemonstrativoPagamentoEventoDto
    {
        public Guid? Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponse MatriculaDemonstrativoPagamento { get; set; }

        public int IdEvento { get; set; }

        public EventoDto Evento { get; set; }

        public decimal? Referencia { get; set; }

        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}