using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFiltrationForCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParams Params)
            : base(p =>
                (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId)
                &&
                (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
            )
        {

        }
    }
}
