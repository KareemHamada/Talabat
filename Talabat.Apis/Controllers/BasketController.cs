using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository) {
            _basketRepository = basketRepository;
        }


        [HttpGet]
        public async Task<ActionResult<CutomerBasket>> GetCustomerBasket(string BasketId) { 
        
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            return Basket is null ? new CutomerBasket(BasketId) : Ok(Basket);
        }


        [HttpPost]
        public async Task<ActionResult<CutomerBasket>> UpdateBasket(CutomerBasket Basket)
        {
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(Basket);
            if (CreatedOrUpdatedBasket is null)
                return BadRequest(new APIResponse(400));

            return Ok(CreatedOrUpdatedBasket);
        }

         
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
