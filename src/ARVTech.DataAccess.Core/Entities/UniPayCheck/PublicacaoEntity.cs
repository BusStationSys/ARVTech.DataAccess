namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PUBLICACOES")]
    public class PublicacaoEntity
    {
        [Description("ID")]
        public virtual int Id { get; set; }

        [Description("TITULO")]
        public virtual string Titulo { get; set; }

        [Description("RESUMO")]
        public virtual string Resumo { get; set; }

        [Description("TEXTO")]
        public virtual string Texto { get; set; }

        public override string ToString()
        {
            return $"Publicação ID: {this.Id}; Título: {this.Titulo}.";
        }
    }
}