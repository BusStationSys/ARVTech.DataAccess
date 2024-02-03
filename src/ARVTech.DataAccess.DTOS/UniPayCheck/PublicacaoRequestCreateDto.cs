namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PublicacaoRequestCreateDto
    {
        public string Titulo { get; set; }

        public string Resumo { get; set; }

        public string Texto { get; set; }

        public byte[] ImageData { get; set; }

        public string ExtensaoConteudo { get; set; }

        public byte[] Conteudo { get; set; }

        public DateTime DataApresentacao { get; set; }

        public DateTime? DataValidade { get; set; }

        public override string ToString()
        {
            return $"Publicação Titulo: {this.Titulo}.";
        }
    }
}