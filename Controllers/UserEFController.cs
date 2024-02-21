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
IUserRepository _userRepository;
IMapper _mapper;
public UserEFController(IConfiguration config, IUserRepository userRepository)
{
_entityFramework = new DataContextEF(config);
_userRepository = userRepository;
_mapper = new Mapper(new MapperConfiguration(cfg =>
cfg.CreateMap<UserToAddDto,User>()
));
}

[HttpGet("GetUsers")]

public IEnumerable<User> GetUsers()
{
return _userRepository.GetUsers();
}
[HttpGet("GetSingleUser/{userId}")]

public User GetSingleUser(int userId)
{
return _userRepository.GetSingleUser(userId);
}
[HttpPut("EditUser")]
public IActionResult EditUser(User user)
{
     User? userDb = _userRepository.GetSingleUser(user.UserId);
     if(userDb != null)
     {
        userDb.Active = user.Active;
        userDb.Gender = user.Gender;
        userDb.Email = user.Email;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        if(_userRepository.SaveChanges())
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
        _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
            throw new Exception("Failed to add user");
            }

            [HttpDelete("DeleteUser/{userId}")]
            public IActionResult DeleteUser(int userId)
            {
     User? userDb = _userRepository.GetSingleUser(userId);
     if(userDb != null)
     {
        _userRepository.RemoveEntity<User>(userDb);
        if (_userRepository.SaveChanges())
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
return _userRepository.GetUserSalary(userId);
}

[HttpPut("EditUserSalary")]
public IActionResult EditUserSalary(UserSalary userSalary)
{
     UserSalary? userSalaryDb = _userRepository.GetUserSalary(userSalary.UserId);
     if(userSalaryDb != null)
     {
        userSalaryDb.Salary = userSalary.Salary;
        if(_userRepository.SaveChanges())
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
       _userRepository.AddEntity<UserSalary>(userSalaryDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
            throw new Exception("Failed to add user salary");
            }

            [HttpDelete("DeleteUserSalary/{userId}")]
            public IActionResult DeleteUserSalary(int userId)
            {
     UserSalary? userSalaryDb = _userRepository.GetUserSalary(userId);
     if(userSalaryDb != null)
     if(userSalaryDb != null)
     {
       _userRepository.RemoveEntity<UserSalary>(userSalaryDb);
        if (_userRepository.SaveChanges())
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
return _userRepository.GetJobInfo(userId);
}

[HttpPut("EditUserJobInfo")]
public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
{
     UserJobInfo? jobInfoDb = _userRepository.GetJobInfo(userJobInfo.UserId);
     if(jobInfoDb != null)
     {
        jobInfoDb.JobTitle = userJobInfo.JobTitle;
        jobInfoDb.Department = userJobInfo.Department;
        if(_userRepository.SaveChanges())
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
       _userRepository.AddEntity<UserJobInfo>(userJobInfoDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
            throw new Exception("Failed to add user job info");
            }

                        [HttpDelete("DeleteUserJobInfo/{userId}")]
            public IActionResult DeleteUserJobInfo(int userId)
            {
     UserJobInfo? userJobInfoDb = _userRepository.GetJobInfo(userId);
     if(userJobInfoDb != null)
     {
       _userRepository.RemoveEntity<UserJobInfo>(userJobInfoDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to delete user job info");
     }
     throw new Exception("Failed to get user job info");
            }
}