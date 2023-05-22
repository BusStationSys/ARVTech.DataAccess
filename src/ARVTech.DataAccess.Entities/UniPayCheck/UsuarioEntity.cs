namespace ARVTech.DataAccess.Entities.UniPayCheck
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

        [Description("USERNAME")]
        public virtual string UserName { get; set; }

        [Description("PASSWORD")]
        public virtual string Password { get; set; }

        [Description("TOKEN")]
        public virtual string Token { get; set; }
    }
}