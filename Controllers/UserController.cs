using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    
    

    [HttpPost("newuser")]
    public IActionResult createUser([FromBody] UserModel user)
    {
     return Ok(new
     {
       message = "User created successfully",
       UserData = user
     });
    }
  }
}