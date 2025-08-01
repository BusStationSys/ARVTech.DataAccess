namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_DEMONSTRATIVOS_PAGAMENTO")]
    public class MatriculaDemonstrativoPagamentoEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDMATRICULA")]
        public Guid GuidMatricula { get; set; }

        public MatriculaEntity Matricula { get; set; }

        [Description("COMPETENCIA")]
        public string Competencia { get; set; }

        public ICollection<MatriculaDemonstrativoPagamentoEventoEntity> MatriculaDemonstrativoPagamentoEventos { get; set; }

        public ICollection<MatriculaDemonstrativoPagamentoTotalizadorEntity> MatriculaDemonstrativoPagamentoTotalizadores { get; set; }

        [Description("DATA_CONFIRMACAO")]
        public DateTimeOffset? DataConfirmacao { get; set; }

        [Description("IP_CONFIRMACAO")]
        public byte[]? IpConfirmacao { get; set; }

        [Description("CONTEUDO_ARQUIVO")]
        public string ConteudoArquivo { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento GUID: {this.Guid}.";
        }
    }
}