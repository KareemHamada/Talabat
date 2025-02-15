
namespace Talabat.Apis.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProdcutToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration) {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProdcutToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl)) {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
