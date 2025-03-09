

using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentIntentIdSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentIdSpec(string PaymentIntentId) :base(O=>O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
