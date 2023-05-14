namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIOS_CABANHAS")]
    public class UsuarioCabanhaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDUSUARIO")]
        public virtual Guid GuidUsuario { get; set; }

        [Description("GUIDCONTA")]
        public virtual Guid GuidConta { get; set; }

        [Description("GUIDCABANHA")]
        public virtual Guid GuidCabanha { get; set; }

        public virtual UsuarioEntity Usuario { get; set; }

        public virtual ContaEntity Conta { get; set; }

        public virtual CabanhaEntity Cabanha { get; set; }

        public override string ToString()
        {
            return $"Usuário Cabanha GUID: {this.Guid}; Usuário: {this.GuidUsuario}; Conta: {this.GuidConta}; Cabanha: {this.GuidCabanha}.";
        }
    }
}