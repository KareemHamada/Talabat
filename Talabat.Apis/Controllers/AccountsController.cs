

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Talabat.Core.Services;

namespace Talabat.Apis.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExists(registerDto.Email).Result.Value)
                return BadRequest(new APIResponse(400, "Email is already in use"));


            var User = new AppUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split('@')[0],
            };

            var Result = await _userManager.CreateAsync(User, registerDto.Password);

            if (!Result.Succeeded) return BadRequest(new APIResponse(400));

            var ReturnedUser = new UserDto()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            };

            return Ok(ReturnedUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User == null) return Unauthorized(new APIResponse(401));


            var Result = await _signInManager.CheckPasswordSignInAsync(User, loginDto.Password, false);
            if (!Result.Succeeded) return Unauthorized(new APIResponse(401));


            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
        }



        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)

            };

            return Ok(ReturnedUser);
        }


        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var returnedAddress = _mapper.Map<Address,AddressDto>(user.Address);
            return Ok(returnedAddress);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto updatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            var mappedAddress = _mapper.Map<AddressDto, Address>(updatedAddress);
            
            mappedAddress.Id = user.Address.Id;

            user.Address = mappedAddress;

            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new APIResponse(400));
            return Ok(updatedAddress);
        }



        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
