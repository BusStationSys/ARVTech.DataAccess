namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MATRICULAS_ESPELHOS_PONTO_MARCACOES")]
    public class MatriculaEspelhoPontoMarcacaoEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDMATRICULA_ESPELHO_PONTO")]
        public virtual Guid GuidMatriculaEspelhoPonto { get; set; }

        [Description("DATA")]
        public virtual DateTime Data { get; set; }

        [Description("MARCACAO")]
        public virtual string Marcacao { get; set; }

        public override string ToString()
        {
            return $"Matrícula Espelho Ponto Marcação GUID: {this.Guid}.";
        }
    }
}