namespace ARVTech.DataAccess.Contracts.PayCheck.Responses
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public record MatriculaDemonstrativoPagamentoEventoResponse
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public int IdEvento { get; set; }

        public EventoResponse Evento { get; set; }

        public decimal? Referencia { get; set; }

        public decimal Valor { get; set; }

        [NotMapped]
        public string ValorFormatado
        {
            get
            {
                //return this.ValorDescriptografado.ToString("#,###,###,##0.00");
                return this.Valor.ToString("#,###,###,##0.00");
            }
        }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}