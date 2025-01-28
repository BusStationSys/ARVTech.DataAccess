namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("USUARIOS_CABANHAS")]
    public class UsuarioCabanhaEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("GUIDUSUARIO")]
        public Guid GuidUsuario { get; set; }

        [Description("GUIDCONTA")]
        public Guid GuidConta { get; set; }

        [Description("GUIDCABANHA")]
        public Guid GuidCabanha { get; set; }

        public UsuarioEntity Usuario { get; set; }

        public ContaEntity Conta { get; set; }

        public CabanhaEntity Cabanha { get; set; }

        public override string ToString()
        {
            return $"Usuário Cabanha GUID: {this.Guid}; Usuário: {this.GuidUsuario}; Conta: {this.GuidConta}; Cabanha: {this.GuidCabanha}.";
        }
    }
}