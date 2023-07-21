namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class LoginDto
    {
        public string EmailUsername { get; set; }

        public string Password { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}