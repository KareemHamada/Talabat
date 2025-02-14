namespace Talabat.Apis.Errors
{
    public class ApiValidationErrorrResponse:APIResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorrResponse():base(400)
        {
            Errors = new List<string>();
        }
    }
}
