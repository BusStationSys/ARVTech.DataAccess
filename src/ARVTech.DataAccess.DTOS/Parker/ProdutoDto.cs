namespace ARVTech.DataAccess.DTOS.Parker
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class responsible for the <see cref="ProdutoDto"/> "Produto" DTO.
    /// </summary>
    public class ProdutoDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProdutoDto"/> class.
        /// </summary>
        public ProdutoDto()
        { }

        /// <summary>
        /// Gets or sets a "Item" field.
        /// </summary>
        [Key]
        public string Item { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a "LinhaProduto" field.
        /// </summary>
        public string LinhaProduto { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a "Composto" field.
        /// </summary>
        public string Composto { get; set; } = string.Empty;
    }
}