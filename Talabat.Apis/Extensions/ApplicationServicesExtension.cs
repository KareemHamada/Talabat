

namespace Talabat.Apis.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services) {


            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddAutoMapper(typeof(MappingProfiles));

            // validation error handling
            Services.Configure<ApiBehaviorOptions>(Options => {

                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors).Select(e => e.ErrorMessage).ToArray();


                    var ValidationErrorResponse = new ApiValidationErrorrResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(ValidationErrorResponse);
                };

            });


            return Services;

        }
    }
}
