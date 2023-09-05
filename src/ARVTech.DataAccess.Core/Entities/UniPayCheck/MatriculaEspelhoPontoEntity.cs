namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHOS_PONTO")]
    public class MatriculaEspelhoPontoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("COMPETENCIA")]
        public virtual string Competencia { get; set; }

        [Description("GUIDMATRICULA")]
        public virtual Guid GuidMatricula { get; set; }

        public virtual MatriculaEntity Matricula { get; set; }

        public ICollection<MatriculaEspelhoPontoCalculoEntity> MatriculaEspelhoPontoCalculos { get; set; }

        public ICollection<MatriculaEspelhoPontoMarcacaoEntity> MatriculaEspelhoPontoMarcacoes { get; set; }

        [Description("DATA_CONFIRMACAO")]
        public virtual DateTimeOffset? DataConfirmacao { get; set; }

        [Description("IP_CONFIRMACAO")]
        public virtual byte[]? IpConfirmacao { get; set; }
        public override string ToString()
        {
            return $"Matrícula Espelho Ponto GUID: {this.Guid}.";
        }
    }
}