namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class AlteracaoSenhaDto
    {
        public Guid GuidUsuario { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento do Password.")]
        [StringLength(32, ErrorMessage = "O Password não pode exceder 32 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "É necessário o preenchimento da Confirmação do Password.")]
        [StringLength(32, ErrorMessage = "A Confirmação do Password não pode exceder 32 caracteres.")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}