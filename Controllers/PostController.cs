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
        if(postId != 0) parameters += ", @PostId = " + postId.ToString();
        if(userId != 0) parameters += ", @UserId = " + userId.ToString();
        if(searchParam.ToLower() != "none") parameters += ", @SearchValue = '" + searchParam + "'";
        if(parameters.Length > 0)sql += parameters[1..];
        return _dapper.LoadData<Post>(sql);

    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> MyPosts()
    {
        string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId = " + 
        User.FindFirst("UserId")?.Value; 
        return _dapper.LoadData<Post>(sql);
    }
    [HttpPut("UpsertPost")]
    public IActionResult UpsertPost(Post post)
    {
        string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
            @UserId = " + User.FindFirst("UserId")?.Value +
            ", @PostTitle = '" + post.PostTitle +
            "', @PostContent = '" + post.PostContent + "'";
            if(post.PostId > 0) sql += ", @PostId = " + post.PostId;
            if(_dapper.ExecuteSql(sql)) return StatusCode(200);
            throw new Exception("Failed to edit post");
    }
    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
        string sql = @"EXEC TutorialAppSchema.spPost_Delete
            @PostId = " + postId.ToString() +
            ", @UserId = " + User.FindFirst("UserId")?.Value;
        if(_dapper.ExecuteSql(sql)) return StatusCode(204);
        throw new Exception("Failed to delete post");
    }
    } 
}