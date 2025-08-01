namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHOS_PONTO")]
    public class MatriculaEspelhoPontoEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("COMPETENCIA")]
        public string Competencia { get; set; }

        [Description("GUIDMATRICULA")]
        public Guid GuidMatricula { get; set; }

        public MatriculaEntity Matricula { get; set; }

        public ICollection<MatriculaEspelhoPontoCalculoEntity> MatriculaEspelhoPontoCalculos { get; set; }

        public ICollection<MatriculaEspelhoPontoMarcacaoEntity> MatriculaEspelhoPontoMarcacoes { get; set; }

        [Description("DATA_CONFIRMACAO")]
        public DateTimeOffset? DataConfirmacao { get; set; }

        [Description("IP_CONFIRMACAO")]
        public byte[]? IpConfirmacao { get; set; }

        [Description("CONTEUDO_ARQUIVO")]
        public string ConteudoArquivo { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}