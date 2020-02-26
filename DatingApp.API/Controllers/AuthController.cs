using System;
using System.Threading.Tasks;
using DatingApp.API.Core.Common;
using DatingApp.API.Core.Users;
using DatingApp.API.Core.Users.DTO;
using DatingApp.API.Models.RequestModel;
using DatingApp.API.Models.ResponseModel;
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
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel loginRequestModel)
        {
            var token = await _userService.Login(new LoginDTO()
            {
                Username = loginRequestModel.Username,
                Password = loginRequestModel.Password
            });
            if (token.IsSuccess)
            {
                return new OkObjectResult(new LoginResponseModel()
                {
                    AccessToken = token.Data.Token
                });
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterRequestModel registerDTO)
        {
            var registerRes = await _userService.Register(new UserRegisterInputDTO()
            {
                Username = registerDTO.Username,
                Password = registerDTO.Password
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