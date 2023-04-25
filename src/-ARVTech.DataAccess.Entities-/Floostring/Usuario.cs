namespace ARVTech.DataAccess.Entities.Floostring
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class responsible for the <see cref="Usuario"/> "Usuários" model.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Usuario"/> class.
        /// </summary>
        public Usuario()
        { }

        /// <summary>
        /// Gets or sets a "Id" field.
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets a "Nome" field.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Nome", Description = "Nome")]
        [Required(ErrorMessage = "É necessário o preenchimento do Nome, verifique!")]
        [StringLength(100, ErrorMessage = "A Descrição deve conter até 100 letras, verifique!")]
        public string Nome { get; set; }

        /// <summary>
        /// Gets or sets a "Login" field.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets a "Celular" field.
        /// </summary>
        public string Celular { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public DateTimeOffset? DataInclusao { get; set; }

        public DateTimeOffset? DataAlteracao { get; set; }

        public DateTimeOffset? DataUltimoLogin { get; set; }

        public string Sobre { get; set; }

        public string Perfil { get; set; }
    }
}