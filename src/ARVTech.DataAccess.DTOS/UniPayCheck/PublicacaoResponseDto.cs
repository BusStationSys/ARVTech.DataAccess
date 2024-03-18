namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using ARVTech.DataAccess.Enums;

    public class PublicacaoResponseDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Resumo { get; set; }

        public string Texto { get; set; }

        public string ExtensaoImagem { get; set; }

        public string NomeImagem { get; set; }

        public byte[] ConteudoImagem { get; set; }

        public string ExtensaoArquivo { get; set; }

        public string NomeArquivo { get; set; }

        public byte[] ConteudoArquivo { get; set; }

        public DateTime DataApresentacao { get; set; }

        public DateTime? DataValidade { get; set; }

        public bool OcultarPublicacao { get; set; }

        public override string ToString()
        {
            return $"Publicação ID: {this.Id}; Título: {this.Titulo}.";
        }
    }
}