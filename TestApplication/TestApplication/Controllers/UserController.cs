using Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApplication.Data;
using TestApplication.Repositories;
using AutoMapper;

namespace TestApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                return Ok(await _userRepository.GetUsers());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Data from Db");
            }
        }
        [AllowAnonymous]
        [HttpGet("/api/GetUsersByDto")]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetUsersByDto()

        {
            var users = await _userRepository.GetUsersForDto();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> Get (int id)
        {
            try
            {
                var result = await _userRepository.GetUser(id);
                if(result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Data from Db");
            }

        }
        [HttpPost]
        public async Task<ActionResult<Users>> CreateUser(Users user)
        {
            try
            {
                if(user == null)
                {
                    return BadRequest();
                }
               var createdUser = await _userRepository.AddUser(user);
                return CreatedAtAction(nameof(GetUsers), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving Data to Db");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Users>> UpdateUser(Users user)
        {
            try
            {
                var Id = user.Id;
                var newUser = await _userRepository.GetUser(Id);
                if (newUser == null)
                {
                    return NotFound();
                }

                return await _userRepository.UpdateUser(newUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving Data to Db");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var userDel = await _userRepository.GetUser(id);
                if (userDel == null)
                {
                    return NotFound();
                }
                await _userRepository.DeleteUser(id);
                return Ok("User deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving Data to Db");
            }
        }
    }
}
