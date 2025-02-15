
namespace Talabat.Apis.Errors
{
    public class ApiExceptionResponse : APIResponse
    {
        public ApiExceptionResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            Details = details; 
        }

        public string? Details { get; set; }
    }
}
