using Microsoft.AspNetCore.Diagnostics;

namespace Talabat.Apis.Errors
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public APIResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            // 500 => internal server error
            // 400 => Bad request
            // 401 => Not authorized
            // 404 => Not found

            return statusCode switch
            {
                400 => "Bad request",
                401 => "You are not authorized",
                404 => "Resourse not found",
                500 => "internal server error",
                _ => null,
            };

        }
    }
}
