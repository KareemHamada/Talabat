
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        // create or update PpaymentIntentId
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string BasketId);
        Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool Flag); 
    }
}
