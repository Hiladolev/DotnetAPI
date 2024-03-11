using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController:ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

    [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
    public IEnumerable<Post> Posts(int postId = 0, int userId = 0, string searchParam = "None")
    {
        string sql = @"EXEC TutorialAppSchema.spPosts_Get";
        string parameters = "";
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@PostIdParameter", postId, DbType.Int64);
        sqlParameters.Add("@UserIdParameter", userId, DbType.Int64);
        sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
        if(postId != 0) parameters += ", @PostId = @PostIdParameter";
        if(userId != 0) parameters += ", @UserId = @UserIdParameter";
        if(searchParam.ToLower() != "none") parameters += ", @SearchValue = @SearchValueParameter";
        if(parameters.Length > 0)sql += parameters[1..];
        return _dapper.LoadDataWithParameters<Post>(sql, sqlParameters);

    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> MyPosts()
    {
        string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId = @UserIdParameter";
        string? userId = User.FindFirst("UserId")?.Value; 
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@UserIdParameter", userId, DbType.String);
        return _dapper.LoadDataWithParameters<Post>(sql,sqlParameters);
    }
    [HttpPut("UpsertPost")]
    public IActionResult UpsertPost(Post post)
    {
        string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
            @UserId = @UserIdParameter,
            @PostTitle = @PostTitleParameter,
            @PostContent = @PostContentParameter";
            string? userId = User.FindFirst("UserId")?.Value;
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", userId, DbType.String);
            sqlParameters.Add("@PostTitleParameter", post.PostTitle, DbType.String);
            sqlParameters.Add("@PostContentParameter", post.PostContent, DbType.String);
            if(post.PostId > 0) 
            {
            sql += ", @PostId = @PostIdParameter";
            sqlParameters.Add("@PostIdParameter", post.PostId, DbType.Int32);
            }
            if(_dapper.ExecuteSqlWithParameters(sql, sqlParameters)) return StatusCode(200);
            throw new Exception("Failed to edit post");
    }
    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
        string sql = @"EXEC TutorialAppSchema.spPost_Delete
            @PostId = @PostIdParameter, @UserId = @UserIdParameter";
        string? userId = User.FindFirst("UserId")?.Value;
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@PostIdParameter", postId, DbType.Int32);
        sqlParameters.Add("@UserIdParameter", userId, DbType.String);
        if(_dapper.ExecuteSqlWithParameters(sql, sqlParameters)) return StatusCode(204);
        throw new Exception("Failed to delete post");
    }
    } 
}