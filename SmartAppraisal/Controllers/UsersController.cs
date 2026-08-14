using BL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SmartAppraisal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDetail user)
        {
            var result = await _userService.CreateAsync(user);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UserDetail user)
        {
            user.Id = id;

            var result = await _userService.UpdateAsync(user);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("User ID and Password are required.");
            }

            var users = await _userService.GetAllAsync();

            var user = users.FirstOrDefault(x =>
                x.UserId == request.UserId &&
                x.Password == request.Password &&
                x.IsActive);

            if (user == null)
            {
                return Unauthorized("Invalid User ID or Password.");
            }

            return Ok(new
            {
                message = "Login successful",
                userId = user.UserId,
                name = user.Name,
                roleId = user.RoleId
            });
        }
    }
}
