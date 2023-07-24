namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UsuarioResponse
    {
        public Guid Guid { get; set; }

        public string Username { get; set; }

        public string IdAspNetUser { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}.";
        }
    }
}