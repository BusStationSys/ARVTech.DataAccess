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

        public string Valor { get; set; }

        [NotMapped]
        public decimal ValorDescriptografado
        {
            get
            {
                //  Atualiza o Valor criptografando a informação usando como chave o GuidMatricula do Demonstrativo de Pagamento Evento.
                var key = this.Guid.ToString("N").ToUpper();

                string normalValue = PasswordCryptography.DecryptString(
                    key,
                    this.Valor);

                if (!string.IsNullOrEmpty(
                    normalValue))
                    return Convert.ToDecimal(
                        normalValue);

                return 0.01M;
            }
        }

        [NotMapped]
        public string ValorFormatado
        {
            get
            {
                return this.ValorDescriptografado.ToString("#,###,###,##0.00");
            }
        }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento Evento GUID: {this.Guid}.";
        }
    }
}