using DotnetAPI.Models;
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

[HttpGet("TextConnection")]
public DateTime TestConnection()
{
    return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
}
[HttpGet("GetUsers")]

public IEnumerable<User> GetUsers()
{
    string sql = @"
        SELECT [UserId],
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active] 
        FROM TutorialAppSchema.Users";

     IEnumerable<User> users = _dapper.LoadData<User>(sql);
return users;
}
[HttpGet("GetSingleUser/{UserId}")]

public User GetSingleUser(int UserId)
{
    string sql = @"
        SELECT [UserId],
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active] 
            FROM TutorialAppSchema.Users 
                WHERE UserId = " + UserId.ToString();

     User user = _dapper.LoadDataSingle<User>(sql);
return user;
}
}