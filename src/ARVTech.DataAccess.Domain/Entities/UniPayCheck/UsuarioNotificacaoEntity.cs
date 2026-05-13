namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vwUsuariosNotificacoes")]
    public class UsuarioNotificacaoEntity
    {
        [Description("TIPO")]
        public string Tipo { get; set; }

        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDUSUARIO")]
        public Guid GuidUsuario { get; set; }

        [Description("GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO")]
        public Guid GuidMatriculaDemonstrativoPagamento { get; set; }

        [Description("GUIDMATRICULA")]
        public Guid GuidMatricula { get; set; }

        [Description("GUIDEMPREGADOR")]
        public Guid GuidEmpregador { get; set; }

        [Description("GUIDCOLABORADOR")]
        public Guid GuidColaborador { get; set; }

        [Description("CONTEUDO")]
        public string Conteudo { get; set; }

        [Description("DATA_ENVIO")]
        public DateTime DataEnvio { get; set; }

        [Description("DATA_LEITURA")]
        public DateTime? DataLeitura { get; set; }

        public override string ToString()
        {
            return $"Tipo: {this.Tipo} GUID: {this.Guid}.";
        }
    }
}