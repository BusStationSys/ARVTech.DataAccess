namespace ARVTech.DataAccess.Entities.EquHos
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PERFIS_CATEGORIAS_USUARIOS")]
    public class PerfilCategoriaUsuarioEntity
    {
        [Key]
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public string Icone { get; set; }
    }
}