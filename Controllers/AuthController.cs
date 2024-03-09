using System.Data;
using System.Security.Cryptography;
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
           byte[] passwordSalt = new byte[128 / 8];
           using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
           {
            rng.GetNonZeroBytes(passwordSalt);
           }
          byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password,passwordSalt);
            string sqlAddAuth = @"EXEC TutorialAppSchema.spRegistration_Upsert
                @Email = @EmailParam,
                @PasswordHash = @PasswordHashParam, 
                @PasswordSalt = @PasswordSaltParam
                )";

           List<SqlParameter> sqlParameters = [];

                SqlParameter emailParameter = new("@EmailParam", SqlDbType.VarChar)
                {
                    Value = userForRegistration.Email
                };
                sqlParameters.Add(emailParameter);
                SqlParameter passwordSaltParameter = new("@PasswordSaltParam", SqlDbType.VarBinary)
                {
                    Value = passwordSalt
                };
                sqlParameters.Add(passwordSaltParameter);

                SqlParameter passwordHashParameter = new("@PasswordHashParam", SqlDbType.VarBinary)
                {
                    Value = passwordHash
                };
                sqlParameters.Add(passwordHashParameter);
                if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters)) 
                {
                    string sqlAddUser = @"
                        INSERT INTO TutorialAppSchema.Users 
                            ([FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active]
                            ) VALUES ('" + userForRegistration.FirstName + 
                        "','" + userForRegistration.LastName +
                        "','" + userForRegistration.Email + 
                        "','" + userForRegistration.Gender +
                        "', 1)";
                        if(_dapper.ExecuteSql(sqlAddUser))return Ok();
                        throw new Exception("Failed to add user");
                }
                throw new Exception("Failed to register user");
            }
                throw new Exception("User password and password confirmation don't match");

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"SELECT 
            [PasswordHash],
            [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email= '" + userForLogin.Email + "'";
            UserForLoginConfirmationDto userForConfirmation = _dapper
            .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);
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