namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HomeController: ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "Hello World" });
    }
    
    [HttpGet("error")]
    public IActionResult GetError()
    {
        throw new Exception("Something went wrong");
    }
}