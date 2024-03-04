using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
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
    string parameters = "";
    if(userId != 0)
    {
    parameters += ", @UserId =" + userId.ToString();
    }
    if(isActive)
    {
    parameters += ", @Active =" + isActive;
    }

    sql += parameters[1..];
    
    IEnumerable<UserComplete> users = _dapper.LoadData<UserComplete>(sql);
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

[HttpPost("AddUser")]
public IActionResult AddUser(UserToAddDto user)
{
     string sql = @"INSERT INTO TutorialAppSchema.Users 
            ([FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
            ) VALUES (
        '" + user.FirstName + 
        "','" + user.LastName +
        "','" + user.Email + 
        "','" + user.Gender +
        "','" + user.Active + "')";
        Console.WriteLine(sql);
        if(_dapper.ExecuteSql(sql)) return Ok();
            throw new Exception("Failed to add user");
            }

            [HttpDelete("DeleteUser/{userId}")]
            public IActionResult DeleteUser(int userId)
            {
                string sql = @"
                DELETE FROM TutorialAppSchema.Users 
                    WHERE UserId = " + userId.ToString();
                Console.WriteLine(sql);
                if(_dapper.ExecuteSql(sql)) return Ok();
                    throw new Exception("Failed to delete user");
            }

//UserSalary
[HttpPut("EditUserSalary")]
public IActionResult EditUserSalary(UserSalary userSalary)
{
    string sql = @"
    UPDATE TutorialAppSchema.UserSalary
        SET [Salary] = '" + userSalary.Salary +
        "' WHERE UserId = " + userSalary.UserId;
        Console.WriteLine(sql);
        if(_dapper.ExecuteSql(sql)) return Ok();
            throw new Exception("Failed to update user salary");
}

[HttpPost("AddUserSalary")]
public IActionResult AddUserSalary(UserSalary userSalary)
{
     string sql = @"INSERT INTO TutorialAppSchema.UserSalary 
            ([UserId],
            [Salary]
            ) VALUES (
        " + userSalary.UserId +","+ userSalary.Salary + ")";
        if(_dapper.ExecuteSql(sql)) return Ok();
            throw new Exception("Failed to add user salary");
            }

            [HttpDelete("DeleteUserSalary/{userId}")]
            public IActionResult DeleteUserSalary(int userId)
            {
                string sql = @"
                DELETE FROM TutorialAppSchema.UserSalary 
                    WHERE UserId = " + userId.ToString();
                Console.WriteLine(sql);
                if(_dapper.ExecuteSql(sql)) return Ok();
                    throw new Exception("Failed to delete user salary");
            }

//UserJobInfo
[HttpPut("EditUserJobInfo")]
public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
{
    string sql = @"
    UPDATE TutorialAppSchema.UserJobInfo
        SET [JobTitle] = '" + userJobInfo.JobTitle +
        "', [Department] = '" + userJobInfo.Department + 
        "' WHERE UserId = " + userJobInfo.UserId;
        Console.WriteLine(sql);
        if(_dapper.ExecuteSql(sql)) return Ok();
            throw new Exception("Failed to update user job info");
}

[HttpPost("AddUserJobInfo")]
public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
{
     string sql = @"INSERT INTO TutorialAppSchema.UserJobInfo 
            ([UserId],
            [JobTitle],
            [Department]
            ) VALUES (
        " + userJobInfo.UserId + 
        ",'" + userJobInfo.JobTitle +
        "','" + userJobInfo.Department +  "')";
     Console.WriteLine(sql);
        if(_dapper.ExecuteSql(sql)) return Ok();
            throw new Exception("Failed to add user job info");
            }

            [HttpDelete("DeleteUserJobInfo/{userId}")]
            public IActionResult DeleteUserJobInfo(int userId)
            {
                string sql = @"
                DELETE FROM TutorialAppSchema.UserJobInfo 
                    WHERE UserId = " + userId.ToString();
                Console.WriteLine(sql);
                if(_dapper.ExecuteSql(sql)) return Ok();
                    throw new Exception("Failed to delete user job info");
            }

}