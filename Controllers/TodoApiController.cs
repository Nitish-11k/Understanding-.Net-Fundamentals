using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers;

  [ApiController]
  [Route("api/[controller]")]
  public class TodoApiController : ControllerBase
{
  
  [HttpGet("message")]
  public IActionResult GetMessage()
  {
    return Ok("Hello from TodoApiController!");
  }

  [HttpPost("echo")]
  public IActionResult EchoMessage([FromBody] string message)
  {
    return Ok($"You sent : {message}");
  }
}
