namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UsuarioRequestCreateDto
    {
        [Required(ErrorMessage = "É necessário o preenchimento do Username.")]
        [StringLength(75, ErrorMessage = "O Username não pode exceder 75 caracteres.")]
        public string Username { get; set; }

        public string Email { get; set; }

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

        public Guid? GuidColaborador { get; set; }

        public DateTimeOffset? DataPrimeiroAcesso { get; set; }

        public override string ToString()
        {
            return $"Usuário Username: {this.Username}.";
        }
    }
}