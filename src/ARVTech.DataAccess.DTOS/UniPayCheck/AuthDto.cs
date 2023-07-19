namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System.ComponentModel.DataAnnotations;

    public class AuthDto
    {
        [Required(ErrorMessage = "É necessário o preenchimento do Username.")]
        [StringLength(32, ErrorMessage = "O tamanho do Username não pode exceder a 32 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "É necessário o preenchimento do Password.")]
        [StringLength(16, ErrorMessage = "O tamanho do Password não pode exceder a 16 caracteres.")]
        public string Password { get; set; }
    }
}