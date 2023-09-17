namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaDemonstrativoPagamentoEventoRequestDto
    {
        public Guid? Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponseDto MatriculaDemonstrativoPagamento { get; set; }

        public int IdEvento { get; set; }

        public EventoRequestDto Evento { get; set; }

        public decimal? Referencia { get; set; }

        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}