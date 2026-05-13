namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using ARVTech.Shared;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MatriculaDemonstrativoPagamentoEventoResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public int IdEvento { get; set; }

        public EventoResponseDto Evento { get; set; }

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