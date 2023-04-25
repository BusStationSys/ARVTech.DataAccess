namespace ARVTech.DataAccess.Entities.Floostring
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    /// <summary>
    /// Class responsible for the <see cref="Grupo"/> "Grupos de Usuários" model.
    /// </summary>
    public class Grupo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grupo"/> class.
        /// </summary>
        public Grupo()
        { }

        /// <summary>
        /// Gets or sets a "Id" field.
        /// </summary>
        [Key]
        public int? Id { get; set; }

        public int? IdUsuarioInclusao { get; set; }

        public int? IdUsuarioAlteracao { get; set; }

        public DateTimeOffset? DataInclusao { get; set; }

        public DateTimeOffset? DataAlteracao { get; set; }

        public string Title { get; set; }

        public string MetaTitle { get; set; }

        public string Slug { get; set; }

        public string Resumo { get; set; }

        public string Status { get; set; }

        public string Perfil { get; set; }

        public string Conteudo { get; set; }

        [ForeignKey("IdUsuarioInclusao")]
        public virtual Usuario UsuarioInclusao { get; set; } = null as Usuario;

        [ForeignKey("IdUsuarioAlteracao")]
        public virtual Usuario UsuarioAlteracao { get; set; } = null as Usuario;
    }
}