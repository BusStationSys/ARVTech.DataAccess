namespace ARVTech.DataAccess.DTOS.Empresarius
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class responsible for the <see cref="ProdutoDto"/> "Produto" model.
    /// </summary>
    public class ProdutoDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProdutoDto"/> class.
        /// </summary>
        public ProdutoDto()
        { }

        /// <summary>
        /// Gets or sets a "Id" field.
        /// </summary>
        [Key]
        public int? Id { get; set; } = null;

        /// <summary>
        /// Gets or sets a "Descricao" field.
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a "Observação" field.
        /// </summary>
        public string Observacao { get; set; } = string.Empty;
    }
}