namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS")]
    public class MatriculaDemonstrativoPagamentoEventoEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO")]
        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        [Description("IDEVENTO")]
        public int IdEvento { get; set; }

        public EventoEntity Evento { get; set; }

        [Description("REFERENCIA")]
        public decimal? Referencia { get; set; }

        [Description("VALOR")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Propriedade temporárias para fallback (não mapeadas para a tabela).
        /// </summary>
        [Description("VALOR_EVENTO")]
        public decimal? ValorEvento { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}