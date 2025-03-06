using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrdreSpecifications : BaseSpecifications<Order>
    {
        // used to get orders for user
        public OrdreSpecifications(string email) : base(o=>o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);

        }

        // used to get order for user
        public OrdreSpecifications(string email, int id) : base(O => O.Id == id && O.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
