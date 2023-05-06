namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;

    public class UsuarioDto
    {
        public Guid? Guid { get; set; }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public override string ToString()
        {
            return $"Usuário Guid: {this.Guid}; Nome: {this.Nome}; Sobrenome: {this.Sobrenome}.";
        }
    }
}