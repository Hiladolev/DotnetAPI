using DotnetAPI.Data;
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

    [HttpGet("Posts")]
    public IEnumerable<Post> Posts()
    {
        string sql = @"SELECT * FROM TutorialAppSchema.Posts";

        return _dapper.LoadData<Post>(sql);

    }
    [HttpGet("PostSingle/{postId}")]
    public Post PostSingle(int postId)
    {
        string sql = @"Select [PostId],
        [UserId],
        [PostTitle],
        [PostContent],
        [PostCreated],
        [PostUpdated] FROM TutorialAppSchema.Posts WHERE PostId = " + postId.ToString(); 
        return _dapper.LoadDataSingle<Post>(sql);
    }
    [HttpGet("MyPosts")]
    public IEnumerable<Post> MyPosts()
    {
        string sql = @"Select [PostId],
        [UserId],
        [PostTitle],
        [PostContent],
        [PostCreated],
        [PostUpdated] FROM TutorialAppSchema.Posts WHERE UserId = " + User.FindFirst("UserId")?.Value; 
        return _dapper.LoadData<Post>(sql);
    }
    } 
}