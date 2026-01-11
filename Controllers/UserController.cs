using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet("allusers")]
    public IActionResult getAllUsers()
    {
      var users = _userService.GetAllUsers();
      return Ok(users);
    }

    [HttpGet("user/{id}")]
    public IActionResult getUserById(int id)
    {
      var user = _userService.GetUserById(id);
      if (user == null)
      {
        return NotFound(new { message = "User not found" });
      }
      return Ok(user);
    }

    

    [HttpPost("newuser")]
    public IActionResult createUser([FromBody] UserModel user)
    {
      var createdUser = _userService.CreateUser(user);
      return Ok(createdUser);
    }
  }
}