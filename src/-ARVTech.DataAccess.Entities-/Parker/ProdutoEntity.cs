namespace ARVTech.DataAccess.Entities.Parker
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblItem")]
    public class ProdutoEntity
    {
        [Key]
        public string Item { get; set; } = string.Empty;

        [Column("Linha de Produto")]
        public string LinhaProduto { get; set; } = string.Empty;

        public string Composto { get; set; } = string.Empty;
    }
}
