namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System.Net;

    public class ApiResponseDto
    {
        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}