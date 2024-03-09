using System.Data;
using System.Security.Cryptography;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if(userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExist = @"SELECT Email FROM TutorialAppSchema.Auth WHERE Email= '" + userForRegistration.Email + "'";
            if(_dapper.ExecuteSql(sqlCheckUserExist))throw new Exception("Email already exist");
            UserForLoginDto userForSetPassword = new(){
                Email = userForRegistration.Email,
                Password = userForRegistration.Password
            };
                if(_authHelper.SetPassword(userForSetPassword)) 
                {
                    string sqlAddUser = @"EXEC TutorialAppSchema.spUser_Upsert
                        @FirstName = '" + userForRegistration.FirstName + 
                        "',@LastName = '" + userForRegistration.LastName + 
                        "', @Email = '" + userForRegistration.Email + 
                        "', @Gender = '" + userForRegistration.Gender + 
                        "', @JobTitle = '" + userForRegistration.JobTitle + 
                        "', @Department = '" + userForRegistration.Department + 
                        "', @Salary = '" + userForRegistration.Salary + 
                        "', @Active = " + 1;
                        if(_dapper.ExecuteSql(sqlAddUser))return Ok();
                        throw new Exception("Failed to add user");
                }
                throw new Exception("Failed to register user");
            }
                throw new Exception("User password and password confirmation don't match");

        }

        [HttpPut("ResetPassword")]

        public IActionResult ResetPassword(UserForLoginDto userForSetPassword)
        {
           if(_authHelper.SetPassword(userForSetPassword))
           {
            return Ok();
           }
           throw new Exception("Failed to reset password");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"EXEC TutorialAppSchema.spLoginConfirmation_Get
            @Email= @EmailParam";

            DynamicParameters sqlParameters = new DynamicParameters();

            sqlParameters.Add("@EmailParam", userForLogin.Email, DbType.String);
            
            UserForLoginConfirmationDto userForConfirmation = _dapper.LoadDataSingleWithParameters<UserForLoginConfirmationDto>(sqlForHashAndSalt,sqlParameters);
            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password,userForConfirmation.PasswordSalt);
            for (int i = 0; i < passwordHash.Length; i++)
            {
                if(passwordHash[i] != userForConfirmation.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }
            string sqlGetUserId = @"
            SELECT UserId
            FROM TutorialAppSchema.Users WHERE email = '" + userForLogin.Email + "'";  
            int userId = _dapper.LoadDataSingle<int>(sqlGetUserId);
            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string sqlGetUserId = @"
            SELECT UserId
            FROM TutorialAppSchema.Users WHERE UserId = '" + User.FindFirst("userId")?.Value + "'"; 

            int userId = _dapper.LoadDataSingle<int>(sqlGetUserId);
            
            return _authHelper.CreateToken(userId); 
        }
    }
}