

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {

        public BasketRepository()
        {
            
        }


        public Task<bool> DeleteBasketAsync(string BasketId)
        {
            throw new NotImplementedException();
        }

        public Task<CutomerBasket> GetBasketAsync(string BasketId)
        {
            throw new NotImplementedException();
        }

        public Task<CutomerBasket> UpdateBasketAsync(CutomerBasket Basket)
        {
            throw new NotImplementedException();
        }
    }
}
