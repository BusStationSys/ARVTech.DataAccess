namespace ARVTech.DataAccess.Contracts.PayCheck.Responses
{
    using System;
    using System.Text.Json;
    using Newtonsoft.Json;

    public record UsuarioResponse
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        public Guid? GuidColaborador { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public PessoaFisicaResponse? Colaborador { get; set; }

        public string? IdAspNetUser { get; set; }

        [JsonProperty("dataPrimeiroAcesso")]
        public DateTimeOffset? DataPrimeiroAcesso { get; set; }

        public int IdPerfilUsuario { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}.";
        }
    }
}