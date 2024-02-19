using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserEFController : ControllerBase
{
DataContextEF _entityFramework;
IMapper _mapper;
public UserEFController(IConfiguration config)
{
_entityFramework = new DataContextEF(config);
_mapper = new Mapper(new MapperConfiguration(cfg =>
cfg.CreateMap<UserToAddDto,User>()
));
}

[HttpGet("GetUsers")]

public IEnumerable<User> GetUsers()
{
     IEnumerable<User> users = _entityFramework.Users.ToList();
return users;
}
[HttpGet("GetSingleUser/{userId}")]

public User GetSingleUser(int userId)
{
     User? user = _entityFramework.Users.Where(user=>user.UserId == userId).FirstOrDefault<User>();
     if(user != null)
     {
        return user;
     }
     throw new Exception("Failed to get user");
}
[HttpPut("EditUser")]
public IActionResult EditUser(User user)
{
     User? userDb = _entityFramework.Users.Where(u=>u.UserId == user.UserId).FirstOrDefault<User>();
     if(userDb != null)
     {
        userDb.Active = user.Active;
        userDb.Gender = user.Gender;
        userDb.Email = user.Email;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        if(_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
            throw new Exception("Failed to update user");
     }
     throw new Exception("Failed to get user");
}

[HttpPost("AddUser")]
public IActionResult AddUser(UserToAddDto user)
{
        User userDb = _mapper.Map<User>(user);
        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
            throw new Exception("Failed to add user");
            }

            [HttpDelete("DeleteUser/{userId}")]
            public IActionResult DeleteUser(int userId)
            {
     User? userDb = _entityFramework.Users.Where(u=>u.UserId == userId).FirstOrDefault<User>();
     if(userDb != null)
     {
        _entityFramework.Users.Remove(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Failed to delete user");
     }
     throw new Exception("Failed to get user");
            }

//UserSalary
[HttpGet("GetUserSalary/{userId}")]

public UserSalary GetUserSalary(int userId)
{
     UserSalary? userSalary = _entityFramework.UserSalary.Where(u=>u.UserId == userId).FirstOrDefault<UserSalary>();
     if(userSalary != null)
     {
        return userSalary;
     }
     throw new Exception("Failed to get user salary");
}

[HttpPut("EditUserSalary")]
public IActionResult EditUserSalary(UserSalary userSalary)
{
     UserSalary? userSalaryDb = _entityFramework.UserSalary.Where(u=>u.UserId == userSalary.UserId).FirstOrDefault<UserSalary>();
     if(userSalaryDb != null)
     {
        userSalaryDb.Salary = userSalary.Salary;
        if(_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
            throw new Exception("Failed to update user salary");
     }
     throw new Exception("Failed to get user salary");
}

[HttpPost("AddUserSalary")]
public IActionResult AddUserSalary(UserSalary userSalary)
{
        UserSalary userSalaryDb = _mapper.Map<UserSalary>(userSalary);
        _entityFramework.Add(userSalaryDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
            throw new Exception("Failed to add user salary");
            }

            [HttpDelete("DeleteUserSalary/{userId}")]
            public IActionResult DeleteUserSalary(int userId)
            {
     UserSalary? userSalaryDb = _entityFramework.UserSalary.Where(u=>u.UserId == userId).FirstOrDefault<UserSalary>();
     if(userSalaryDb != null)
     {
        _entityFramework.UserSalary.Remove(userSalaryDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Failed to delete user");
     }
     throw new Exception("Failed to get user");
            }

            //UserJobInfo
[HttpGet("GetJobInfo/{userId}")]

public UserJobInfo GetJobInfo(int userId)
{
     UserJobInfo? userJobInfo = _entityFramework.UserJobInfo.Where(user=>user.UserId == userId).FirstOrDefault<UserJobInfo>();
     if(userJobInfo != null)
     {
        return userJobInfo;
     }
     throw new Exception("Failed to get user job info");
}

[HttpPut("EditUserJobInfo")]
public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
{
     UserJobInfo? jobInfoDb = _entityFramework.UserJobInfo.Where(u=>u.UserId == userJobInfo.UserId).FirstOrDefault<UserJobInfo>();
     if(jobInfoDb != null)
     {
        jobInfoDb.JobTitle = userJobInfo.JobTitle;
        jobInfoDb.Department = userJobInfo.Department;
        if(_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
            throw new Exception("Failed to update user job info");
     }
     throw new Exception("Failed to get user job info");
}


[HttpPost("AddUserJobInfo")]
public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
{
        UserJobInfo userJobInfoDb = _mapper.Map<UserJobInfo>(userJobInfo);
        _entityFramework.Add(userJobInfoDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
            throw new Exception("Failed to add user job info");
            }

                        [HttpDelete("DeleteUserJobInfo/{userId}")]
            public IActionResult DeleteUserJobInfo(int userId)
            {
     UserJobInfo? userJobInfoDb = _entityFramework.UserJobInfo.Where(u=>u.UserId == userId).FirstOrDefault<UserJobInfo>();
     if(userJobInfoDb != null)
     {
        _entityFramework.UserJobInfo.Remove(userJobInfoDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Failed to delete user job info");
     }
     throw new Exception("Failed to get user job info");
            }
}