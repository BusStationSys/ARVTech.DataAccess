namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.Text.Json;
    using Newtonsoft.Json;

    public class UsuarioResponse : ApiResponse
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        public Guid? GuidColaborador { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public PessoaFisicaResponse Colaborador { get; set; }

        public string? IdAspNetUser { get; set; }

        [JsonProperty("dataPrimeiroAcesso")]
        public DateTimeOffset? DataPrimeiroAcesso { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}.";
        }
    }
}