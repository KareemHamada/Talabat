using AutoMapper;
using Talabat.Apis.DTOS;
using Talabat.Core.Entities;

namespace Talabat.Apis.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() {

            CreateMap<Product, ProdcutToReturnDto>()
                .ForMember(a=>a.ProductType, o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(a => a.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(a => a.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());


        }
    }
}
