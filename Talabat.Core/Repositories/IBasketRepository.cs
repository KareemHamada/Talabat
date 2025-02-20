



namespace Talabat.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<CutomerBasket?> GetBasketAsync(string BasketId);
        Task<CutomerBasket?> UpdateBasketAsync(CutomerBasket Basket);
        Task<bool> DeleteBasketAsync(string BasketId);
    }
}
