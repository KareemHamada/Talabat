﻿namespace Talabat.Apis.DTOS
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDto ShipToAddress { get; set; }

    }
}
