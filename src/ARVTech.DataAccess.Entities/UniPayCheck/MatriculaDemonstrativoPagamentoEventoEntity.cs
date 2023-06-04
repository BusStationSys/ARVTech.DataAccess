namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS")]
    public class MatriculaDemonstrativoPagamentoEventoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO")]
        public virtual Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public virtual MatriculaDemonstrativoPagamentoEntity MatriculaDemonstrativoPagamento { get; set; }

        [Description("IDEVENTO")]
        public virtual int IdEvento { get; set; }

        public virtual EventoEntity Evento { get; set; }

        [Description("REFERENCIA")]
        public virtual decimal Referencia { get; set; }

        [Description("VALOR")]
        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}