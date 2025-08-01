namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIOS")]
    public class UsuarioEntity
    {
        [Description("GUID")]
        public Guid? Guid { get; set; }

        [Description("GUIDCOLABORADOR")]
        public Guid? GuidColaborador { get; set; }

        public PessoaFisicaEntity Colaborador { get; set; }

        [Description("EMAIL")]
        public string Email { get; set; }

        [Description("USERNAME")]
        public string Username { get; set; }

        [Description("PASSWORD")]
        public string Password { get; set; }

        [Description("IDASPNETUSER")]
        public int? IdAspNetUser { get; set; }

        [Description("DATA_PRIMEIRO_ACESSO")]
        public DateTimeOffset? DataPrimeiroAcesso { get; set; }

        [Description("IDPERFIL_USUARIO")]
        public int IdPerfilUsuario { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}; Username: {this.Username}.";
        }
    }
}