namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class MatriculaDemonstrativoPagamentoDto
    {
        public Guid? Guid { get; set; }

        public Guid? GuidMatricula { get; set; }

        public string Competencia { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento GUID: {this.Guid}.";
        }
    }
}