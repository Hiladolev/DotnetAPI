using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserCompleteController : ControllerBase
{
DataContextDapper _dapper;
public UserCompleteController(IConfiguration config)
{
_dapper = new DataContextDapper(config);
}

[HttpGet("TextConnection")]
public DateTime TestConnection()
{
    return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
}
[HttpGet("GetUsers/{userId}/{isActive}")]

public IEnumerable<UserComplete> GetUsers(int userId,bool isActive)
{
    string sql = @"EXEC TutorialAppSchema.spUsers_Get";
    string stringParameters = "";
    DynamicParameters sqlParameters = new DynamicParameters();
    if(userId != 0)
    {
    stringParameters += ", @UserId = @UserIdParameter";
    sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
    }
    if(isActive)
    {
    stringParameters += ", @Active = @ActiveParameter";
    sqlParameters.Add("@ActiveParameter", isActive, DbType.Boolean);
    }

    if(stringParameters.Length > 0)
    {
        sql += stringParameters[1..];
    }
    
    IEnumerable<UserComplete> users = _dapper.LoadDataWithParameters<UserComplete>(sql, sqlParameters);
    return users;
}
[HttpPut("UpsertUser")]
public IActionResult UpsertUser(UserComplete user)
{
    string sql = @"EXEC TutorialAppSchema.spUser_Upsert
    @FirstName = '" + user.FirstName + 
    "',@LastName = '" + user.LastName + 
    "', @Email = '" + user.Email + 
    "', @Gender = '" + user.Gender + 
    "', @JobTitle = '" + user.JobTitle + 
    "', @Department = '" + user.Department + 
    "', @Salary = '" + user.Salary + 
    "', @Active = '" + user.Active + 
    "', @UserId = " + user.UserId ;
        if(_dapper.ExecuteSql(sql)) return Ok();
            throw new Exception("Failed to update user");
}

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = @"
            EXEC TutorialAppSchema.spUser_Delete
                    @UserId = " + userId.ToString();
            if(_dapper.ExecuteSql(sql)) return Ok();
            throw new Exception("Failed to delete user");
        }
}