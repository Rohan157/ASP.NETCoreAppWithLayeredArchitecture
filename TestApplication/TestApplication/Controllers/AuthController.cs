using Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TestApplication.Data;
using TestApplication.Repositories;

namespace JwtWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Login>> Register(LoginDto request)
        {
            try
            {
                if(request == null)
                {
                    return BadRequest("User is null");
                }
                var loggedInUser = await _authRepository.UserRegister(request);
                return(loggedInUser);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving Data to Db");
            }


        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("User is null");
                }
                var loggedInUser = await _authRepository.LoginUser(request);
                return (loggedInUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving Data to Db");
            }
        }       
    }
}