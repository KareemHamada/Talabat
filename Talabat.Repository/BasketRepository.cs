

using System.Text.Json;
using StackExchange.Redis;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }


        public Task<bool> DeleteBasketAsync(string BasketId)
        {
            return _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CutomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket = await _database.StringGetAsync(BasketId);
            return Basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CutomerBasket>(Basket);
        }

        public async Task<CutomerBasket?> UpdateBasketAsync(CutomerBasket Basket)
        {
            var JsonBasket = JsonSerializer.Serialize(Basket);
            var IsCreatedOrUpdated = await _database.StringSetAsync(Basket.Id, JsonBasket, TimeSpan.FromDays(1));

            if (!IsCreatedOrUpdated) return null;

            return await GetBasketAsync(Basket.Id);
        }
    }
}
