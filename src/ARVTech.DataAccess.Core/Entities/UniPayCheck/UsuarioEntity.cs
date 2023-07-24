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
        public virtual Guid GuidColaborador { get; set; }

        public virtual PessoaFisicaEntity Colaborador { get; set; }

        [Description("USERNAME")]
        public virtual string UserName { get; set; }

        public override string ToString()
        {
            return $"Usuário GUID: {this.Guid}; Username: {this.UserName}.";
        }
    }
}