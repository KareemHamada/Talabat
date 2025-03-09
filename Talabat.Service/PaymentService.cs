using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Forwarding;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Product = Talabat.Core.Entities.Product;
using Stripe.Checkout;
using Microsoft.AspNetCore.Http;
using Talabat.Core.Specifications.Order_Spec;



namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const string Secret = "your_stripe_webhook_secret"; // Set this to your webhook secret

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            this._configuration = configuration;
            this._basketRepository = basketRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSetting:Secretkey"];

            // Get basket
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;

            // Amount = SubTotal + DM.Cost
            var ShippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);

                ShippingPrice = DeliveryMethod.Cost;
            }

            if (Basket.Items.Count > 0) {

                foreach (var item in Basket.Items) {

                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }

            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            var Service = new PaymentIntentService();

            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))// Create
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) SubTotal * 100 + (long) ShippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card"}
                };

                paymentIntent = await Service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long) SubTotal *100 + (long) ShippingPrice * 100,
                };
                paymentIntent =   await Service.UpdateAsync(Basket.PaymentIntentId,Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret= paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateBasketAsync(Basket);

            return Basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool Flag)
        {
            var spec = new OrderWithPaymentIntentIdSpec(PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityAsyncWithSpecifications(spec);


            if (Flag)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status =OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();


            return order;
        }
    }
}
