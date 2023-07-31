namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;

    public class UsuarioResponse: ApiResponse
    {
        public Guid Guid { get; set; }

        public Guid? GuidColaborador { get; set; }

        public string Username { get; set; }

        public PessoaFisicaResponse Colaborador { get; set; }

        public string? IdAspNetUser { get; set; }

        public DateTimeOffset? DataPrimeiroAcesso { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}.";
        }
    }
}