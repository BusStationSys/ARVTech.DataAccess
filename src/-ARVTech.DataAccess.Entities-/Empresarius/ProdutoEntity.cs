namespace ARVTech.DataAccess.Entities.Empresarius
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRODUTOS")]
    public class ProdutoEntity
    {
        [Key]
        [Column("IDPRODUTO")]
        public decimal? Id { get; set; } = null;

        [Key]
        [Column("IDEMPRESA")]
        public decimal? IdEmpresa { get; set; } = null;

        [Column("DESCRICAO")]
        public string Descricao { get; set; } = string.Empty;

        [Column("PRECO_VENDA")]
        public double? PrecoVenda { get; set; } = null;

        [Column("ESTOQUE_MINIMO")]
        public double? EstoqueMinimo { get; set; } = null;

        [Column("PRECO_CUSTO")]
        public double? PrecoCusto { get; set; } = null;

        [Column("CUSTO_MEDIO")]
        public double? CustoMedio { get; set; } = null;

        [Column("MARGEM_LUCRO")]
        public double? MargemLucro { get; set; } = null;

        [Column("OBSERVACAO")]
        public string Observacao { get; set; } = string.Empty;
    }
}