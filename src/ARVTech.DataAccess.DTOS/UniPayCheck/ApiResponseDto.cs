namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System.Net;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
}