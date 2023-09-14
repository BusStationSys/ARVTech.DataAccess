namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaDemonstrativoPagamentoTotalizadorDto
    {
        public Guid? Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponse MatriculaDemonstrativoPagamento { get; set; }

        public int IdTotalizador { get; set; }

        public TotalizadorDto Totalizador { get; set; }

        public virtual decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Totalizador GUID: {this.Guid}.";
        }
    }
}