
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using StackExchange.Redis;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.Apis.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService,IMapper mapper,IUnitOfWork unitOfWork) {
            _orderService = orderService;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(Core.Entities.Order_Aggregate.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Core.Entities.Order_Aggregate.Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto,Core.Entities.Order_Aggregate.Address>(orderDto.ShipToAddress);

            var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if (Order is null) return BadRequest(new APIResponse(400, "There is a problem with your order"));

            return Ok(Order);
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new APIResponse(404, "There is no orders for this user"));


            var MappedOrder = _mapper.Map<IReadOnlyList<Core.Entities.Order_Aggregate.Order>, IReadOnlyList<OrderToReturnDto>>(Orders);

            return Ok(MappedOrder);
        }



        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, id);
            if (order is null) return NotFound(new APIResponse(404, $"There is no orders with id = {id} for this user"));

            var MappedOrder = _mapper.Map<Core.Entities.Order_Aggregate.Order, OrderToReturnDto>(order);

            return Ok(MappedOrder);
        }

        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliverMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            if (deliverMethods is null) return NotFound(new APIResponse(404, "There is no delivery methods"));

            return Ok(deliverMethods);
        }
    }
}
