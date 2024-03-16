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

        [Description("DATA_APRESENTACAO")]
        public virtual DateTime DataApresentacao { get; set; }

        [Description("DATA_VALIDADE")]
        public virtual DateTime? DataValidade { get; set; }

        [Description("EXTENSAO_IMAGEM")]
        public virtual string? ExtensaoImagem { get; set; }

        [Description("CONTEUDO_IMAGEM")]
        public virtual byte[]? ConteudoImagem { get; set; }

        [Description("NOME_IMAGEM")]
        public virtual string? NomeImagem { get; set; }

        [Description("EXTENSAO_ARQUIVO")]
        public virtual string? ExtensaoArquivo { get; set; }

        [Description("CONTEUDO_ARQUIVO")]
        public virtual byte[]? ConteudoArquivo { get; set; }

        [Description("NOME_ARQUIVO")]
        public virtual string? NomeArquivo { get; set; }

        [Description("OCULTAR_PUBLICACAO")]
        public virtual bool OcultarPublicacao { get; set; }

        public override string ToString()
        {
            return $"Publicação ID: {this.Id}; Título: {this.Titulo}.";
        }
    }
}