

namespace Talabat.Core
{
    public class ProdcutWithBrandAndTypeSpecifications: BaseSpecifications<Product>
    {
        public ProdcutWithBrandAndTypeSpecifications(ProductSpecParams Params) 
            :base(p=> 
                (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
                &&
                (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId) 
                && 
                (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
            ) 
        {
            Includes.Add(p=>p.ProductType);
            Includes.Add(p => p.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
                
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }

        // ctor is used for get product by id
        public ProdcutWithBrandAndTypeSpecifications(int id):base(p=>p.Id == id)
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);
        }

    }
}
