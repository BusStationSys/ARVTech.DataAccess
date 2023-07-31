namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIOS")]
    public class UsuarioEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDCOLABORADOR")]
        public virtual Guid? GuidColaborador { get; set; }

        public virtual PessoaFisicaEntity Colaborador { get; set; }

        [Description("USERNAME")]
        public virtual string UserName { get; set; }

        [Description("PASSWORD")]
        public virtual string Password { get; set; }

        [Description("IDASPNETUSER")]
        public virtual int? IdAspNetUser { get; set; }

        [Description("DATA_PRIMEIRO_ACESSO")]
        public virtual DateTimeOffset? DataPrimeiroAcesso { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}; Username: {this.UserName}.";
        }
    }
}