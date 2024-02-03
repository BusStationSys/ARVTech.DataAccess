﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class PublicacaoResponseDto
    {
        public int Id { get; set; }

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
            return $"Publicação ID: {this.Id}; Título: {this.Titulo}.";
        }
    }
}