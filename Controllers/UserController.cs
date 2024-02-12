using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
DataContextDapper _dapper;
public UserController(IConfiguration config)
{
_dapper = new DataContextDapper(config);
}

[HttpGet("GetUsers/{testValue}")]

public string[] GetUsers(string testValue)
{
    return ["user1","user2",testValue];
}
}