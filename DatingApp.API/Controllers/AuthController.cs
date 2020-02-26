using System;
using System.Threading.Tasks;
using DatingApp.API.Core.Common;
using DatingApp.API.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBaseCommon
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            this._userService = userService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AccessToken>> Login(LoginDTO loginDTO)
        {
            try
            {

                var token = await _userService.Login(loginDTO.UserName, loginDTO.Password);
                if (token.IsSuccess)
                {
                    return new OkObjectResult(token.Data);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(CreateUserDTO createUserDTO)
        {
            var registerRes = await _userService.Register(new UserRegisterInputDTO()
            {
                UserName = createUserDTO.UserName,
                Password = createUserDTO.PassWord
            });
            if (registerRes.IsSuccess)
            {
                return StatusCode(201);
            }
            else
            {
                return BadRequest(registerRes.ErrorMessage);
            }
        }
    }
}