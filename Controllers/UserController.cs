using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{

public UserController(IConfiguration config)
{
    Console.WriteLine(config.GetConnectionString("DefaultConnection"));
}

[HttpGet("GetUsers/{testValue}")]

public string[] GetUsers(string testValue)
{
    return ["user1","user2",testValue];
}
}