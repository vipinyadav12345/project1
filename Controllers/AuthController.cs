using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository  authRepo)
        {
           _authRepo = authRepo;
        }


        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto  request)
        {
            var response = await _authRepo.Register(
                new User{UserName =request.Username}, request.Password
            );
            if(!response.success){
                return BadRequest(response);
            }
            return Ok(response.Data);
        }
      [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto  request)
        {
            var response = await _authRepo.Login(request.Username , request.Password);
            if(!response.success){
                return BadRequest(response);
            }
            return Ok(response.Data);
        }
    }
}