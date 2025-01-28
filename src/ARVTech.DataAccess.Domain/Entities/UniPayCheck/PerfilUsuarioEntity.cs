namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PERFIS_USUARIOS")]
    public class PerfilUsuarioEntity
    {
        [Description("ID")]
        public int Id { get; set; }

        [Description("DESCRICAO")]
        public string Descricao { get; set; }

        public override string ToString()
        {
            return $"Perfil de Usuário ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}