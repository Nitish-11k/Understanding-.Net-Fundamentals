using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TodoController : ControllerBase
  {
    [HttpGet("public")]
    public IActionResult PublicEndpoint()
    {
      return Ok("This is a public endpoint.");
    }

    [HttpGet("protected")]
    [Authorize]
    public IActionResult ProtectedEndpoint()
    {
      return Ok("This is a protected endpoint.");
    } 
  }
}