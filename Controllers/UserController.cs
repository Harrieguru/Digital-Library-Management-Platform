using eBook_manager.Models;
using eBook_manager.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eBook_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound(); // Returns 404 if user not found
            }
            return Ok(user); // Returns user details if found
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users); // Returns all users
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser(UserDTO user)
        {
            if (user == null)
            {
                return BadRequest(); // Returns 400 if user data is not provided
            }

            await _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = user.email }, user); // Returns 201 and the created user
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UserDTO user)
        {
            if (user == null || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid user data or email"); // Returns 400 if user data is not provided
            }

            if (email != user.email)
            {
                return BadRequest("Email in the URL does not match user data"); // Handle email mismatch
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors if any
            }

            var userExists = await _userRepository.GetUserByEmail(email);
            if (userExists == null)
            {
                return NotFound("User not found"); // Return 404 if user does not exist
            }

            await _userRepository.UpdateUser(email, user);
            return NoContent(); // Returns 204 for a successful update
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var existingUser = await _userRepository.GetUserByEmail(email);
            if (existingUser == null)
            {
                return NotFound(); // Returns 404 if user not found
            }

            await _userRepository.DeleteUserByEmail(email);
            return NoContent(); // Returns 204 for successful deletion
        }
    }
}
