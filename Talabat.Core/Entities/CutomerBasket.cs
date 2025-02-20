

namespace Talabat.Core.Entities
{
    public class CutomerBasket 
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }

        public CutomerBasket(string id) {
            Id = id;
        }
    }
}
