namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MatriculaDemonstrativoPagamentoTotalizadorResponseDto
    {
        public Guid Guid { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public MatriculaDemonstrativoPagamentoResponseDto MatriculaDemonstrativoPagamento { get; set; }

        public int IdTotalizador { get; set; }

        public TotalizadorResponseDto Totalizador { get; set; }

        public decimal Valor { get; set; }

        [NotMapped]
        public string ValorFormatado
        {
            get
            {
                //  this.SalarioNominal.ToString("#,##0.00", new CultureInfo("pt-BR"))
                return this.Valor.ToString("#,###,###,##0.00");

                // Fallback para Valor do Totalizador.
                    // Since Valor is decimal and ValorTotalizador is nullable decimal, use null-coalescing operator.
                    //if (this.ValorTotalizador.HasValue &&
                    //    this.Valor == 0)
                    //    this.Valor = this.ValorTotalizador.Value;
            }
        }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Totalizador GUID: {this.Guid}.";
        }
    }
}