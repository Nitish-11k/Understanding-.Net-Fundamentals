using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using System.Threading.Tasks;

namespace Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
      _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserModel user)
    {
      var createdUser = await _userService.CreateUserAsync(user);
      return Ok(createdUser);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await _userService.GetAllUserAsync();
      return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      if (user == null)
      {
        return NotFound(new { message = "User not found" });
      }
      return Ok(user);
    }

    

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id , UserModel user)
    {
      if(id != user.Id)
      {
        return BadRequest(new { message = "User ID mismatch" });
      }
      var updatedUser = await _userService.UpdateUserAsync(user);
      if (updatedUser == null)
      {
        return NotFound(new { message = "User not found" });  
      }
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var isDeleted = await _userService.DeleteUserAsync(id);
      if (!isDeleted)
      {
        return NotFound(new { message = "User not found" });  
      }
      return NoContent();
    }
  }
}