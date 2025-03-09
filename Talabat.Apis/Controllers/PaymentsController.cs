using Microsoft.AspNetCore.Authorization;
using Stripe;
using Talabat.Core.Services;

namespace Talabat.Apis.Controllers
{
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endPointSecret = "endpointSecret";
        public PaymentsController(IPaymentService paymentService,IMapper mapper)
        {
            this._paymentService = paymentService;
            this._mapper = mapper;
        }

        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new APIResponse(400, "There is a problem with your basket"));

            var MappedBasket = _mapper.Map<CustomerBasket,CustomerBasketDto>(basket);
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endPointSecret);


                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;

                //handle the event
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                   await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, false);
                }
                else if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, true);

                }
                //else
                //{
                //    Console.WriteLine("Unhandled event type {0}",stripeEvent.Type);
                //}

                return Ok(stripeEvent);
            }
            catch (StripeException ex) {
            
                return BadRequest();
            }
        }
    }
}
