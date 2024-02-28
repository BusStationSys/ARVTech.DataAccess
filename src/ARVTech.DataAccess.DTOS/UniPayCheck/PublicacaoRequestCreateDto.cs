namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PublicacaoRequestCreateDto
    {
        [StringLength(150, ErrorMessage = "O Título não pode exceder 150 caracteres.", MinimumLength = 0)]
        public string Titulo { get; set; }

        public string Resumo { get; set; }

        public string Texto { get; set; }

        public string? ExtensaoImagem { get; set; }

        public byte[]? ConteudoImagem { get; set; }

        public string? ExtensaoArquivo { get; set; }

        public byte[]? ConteudoArquivo { get; set; }

        public DateTime DataApresentacao { get; set; }

        public DateTime? DataValidade { get; set; }

        public bool OcultarPublicacao { get; set; }

        public override string ToString()
        {
            return $"Publicação Titulo: {this.Titulo}.";
        }
    }
}