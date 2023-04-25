namespace ARVTech.DataAccess.Entities.EquHos
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TIPOS")]
    public class TipoEntity
    {
        [Key]
        public int? Id { get; set; } = null;

        [Column("EXIBIR_QUADRO_ANIMAIS")]
        public bool ExibirQuadroAnimais { get; set; } = false;

        [Column("_QUANTIDADE_ANIMAIS")]
        public int QuantidadeAnimais { get; set; } = 0;

        public string Cor { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Icone { get; set; } = string.Empty;

        public string Observacoes { get; set; } = string.Empty;

        public string Sexo { get; set; } = "M";
    }
}