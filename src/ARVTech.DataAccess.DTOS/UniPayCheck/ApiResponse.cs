﻿namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System.Net;

    public class ApiResponse
    {
        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}