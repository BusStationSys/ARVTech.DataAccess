namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaDemonstrativoPagamentoRequestCreateDto
    {
        public Guid? Guid { get; set; }

        public Guid? GuidMatricula { get; set; }

        public string Competencia { get; set; }

        public DateTimeOffset? DataConfirmacao { get; set; }

        public byte[]? IpConfirmacao { get; set; }

        public string ConteudoArquivo { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento GUID: {this.Guid}.";
        }
    }
}