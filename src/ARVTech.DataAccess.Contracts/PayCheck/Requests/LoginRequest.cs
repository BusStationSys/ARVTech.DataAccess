namespace ARVTech.DataAccess.Contracts.PayCheck.Requests
{
    public class LoginRequest
    {
        public string CpfEmailUsername { get; set; }

        public string Password { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}