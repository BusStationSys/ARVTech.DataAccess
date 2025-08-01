namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PUBLICACOES")]
    public class PublicacaoEntity
    {
        [Description("ID")]
        public int Id { get; set; }

        [Description("TITULO")]
        public string Titulo { get; set; }

        [Description("RESUMO")]
        public string Resumo { get; set; }

        [Description("TEXTO")]
        public string Texto { get; set; }

        [Description("DATA_APRESENTACAO")]
        public DateTime DataApresentacao { get; set; }

        [Description("DATA_VALIDADE")]
        public DateTime? DataValidade { get; set; }

        [Description("EXTENSAO_IMAGEM")]
        public string? ExtensaoImagem { get; set; }

        [Description("CONTEUDO_IMAGEM")]
        public byte[]? ConteudoImagem { get; set; }

        [Description("NOME_IMAGEM")]
        public string? NomeImagem { get; set; }

        [Description("EXTENSAO_ARQUIVO")]
        public string? ExtensaoArquivo { get; set; }

        [Description("CONTEUDO_ARQUIVO")]
        public byte[]? ConteudoArquivo { get; set; }

        [Description("NOME_ARQUIVO")]
        public string? NomeArquivo { get; set; }

        [Description("OCULTAR_PUBLICACAO")]
        public bool OcultarPublicacao { get; set; }

        public override string ToString()
        {
            return $"Publicação ID: {this.Id}; Título: {this.Titulo}.";
        }
    }
}