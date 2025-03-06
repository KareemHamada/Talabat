
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Apis.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() {

            CreateMap<Product, ProdcutToReturnDto>()
                .ForMember(a=>a.ProductType, o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(a => a.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(a => a.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                    .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                    .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                    .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                    .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                    .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                    .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();

        }
    }
}
