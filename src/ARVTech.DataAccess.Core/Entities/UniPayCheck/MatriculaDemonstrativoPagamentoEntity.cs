namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_DEMONSTRATIVOS_PAGAMENTO")]
    public class MatriculaDemonstrativoPagamentoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA")]
        public virtual Guid GuidMatricula { get; set; }

        public virtual MatriculaEntity Matricula { get; set; }

        [Description("COMPETENCIA")]
        public virtual string Competencia { get; set; }

        public ICollection<MatriculaDemonstrativoPagamentoEventoEntity> MatriculaDemonstrativoPagamentoEventos { get; set; }

        public ICollection<MatriculaDemonstrativoPagamentoTotalizadorEntity> MatriculaDemonstrativoPagamentoTotalizadores { get; set; }

        [Description("DATA_CONFIRMACAO")]
        public virtual DateTimeOffset? DataConfirmacao { get; set; }

        [Description("IP_CONFIRMACAO")]
        public virtual byte[]? IpConfirmacao { get; set; }

        [Description("CONTEUDO_ARQUIVO")]
        public virtual string ConteudoArquivo { get; set; }

        public override string ToString()
        {
            return $"Matrícula Demonstrativo Pagamento GUID: {this.Guid}.";
        }
    }
}