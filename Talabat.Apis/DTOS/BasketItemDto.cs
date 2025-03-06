namespace Talabat.Apis.DTOS
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price can not be 0")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue,ErrorMessage ="Quantity must be one item at liast")]
        public int Quantity { get; set; }
    }
}