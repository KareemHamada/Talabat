
namespace Talabat.Core.Specifications
{
    public class ProductWithFiltrationForCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParams Params)
            : base(p =>
                (!string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
                &&
                (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId)
                &&
                (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
            )
        {

        }
    }
}
