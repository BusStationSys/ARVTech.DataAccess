namespace ARVTech.DataAccess.Contracts.PayCheck.Responses
{
    using System;

    public record UsuarioNotificacaoResponse
    {
        public string Tipo { get; set; }

        public Guid Guid { get; set; }

        public Guid GuidUsuario { get; set; }

        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        public Guid GuidMatricula { get; set; }

        public Guid GuidEmpregador { get; set; }

        public Guid GuidColaborador { get; set; }

        public string Conteudo { get; set; }

        public DateTime DataEnvio { get; set; }

        public DateTime? DataLeitura { get; set; }

        public override string ToString()
        {
            return $"Tipo: {this.Tipo} GUID: {this.Guid}.";
        }
    }
}