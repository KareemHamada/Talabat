using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            this._paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Core.Entities.Order_Aggregate.Address ShippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(BasketId);

            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();

            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProdcutItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProdcutItemOrdered,item.Quantity, Product.Price);

                    OrderItems.Add(OrderItem);
                }
            }
            //3.Calculate SubTotal
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            //5.Create Order
            var Spec = new OrderWithPaymentIntentIdSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityAsyncWithSpecifications(Spec);
            if(ExOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }
            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal,Basket.PaymentIntentId);

            //6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(Order);

            //7.Save Order To Database[ToDo]

            var Result = await _unitOfWork.CompleteAsync();


            if (Result <= 0)
                return null;

            return Order;
        }

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
        {
            var spec = new OrderSpecifications(BuyerEmail,OrderId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityAsyncWithSpecifications(spec);

            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail)
        {
            var OrderSpec = new OrderSpecifications(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllAsyncWithSpecifications(OrderSpec);

            return Orders;
        }


    }
}
