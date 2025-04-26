using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zimozi.Assessment.BusinessModels.RequestModels.Login;
using Zimozi.Assessment.BusinessModels.ResponseModels.Login;
using Zimozi.Assessment.BusinessModels.ResponseModels;
using Zimozi.Assessment.Services.UserService;

namespace Zimozi.Assessment.Controller
{
    [Route("api/users")]
    [ApiController]
    public class AuthencationController : ControllerBase
    {
        private IUserService _userService;
        public AuthencationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BusinessResult<LoginResponse>
                {
                    Message = "Dữ liệu không hợp lệ",
                    StatusCode = 400,
                    Data = null
                });
            }
            var result = await _userService.Login(model);

            return StatusCode(result.StatusCode, result);

        }
    }
}
