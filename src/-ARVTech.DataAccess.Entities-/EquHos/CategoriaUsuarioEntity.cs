namespace ARVTech.DataAccess.Entities.EquHos
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CATEGORIAS_USUARIOS")]
    public class CategoriaUsuarioEntity
    {
        [Key]
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public int? Perfil { get; set; } = null;

        public virtual PerfilCategoriaUsuarioEntity PerfilCategoriaUsuario { get; set; }
    }
}