﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class LoginRequestDto
    {
        public string CpfEmailUsername { get; set; }

        public string Password { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}