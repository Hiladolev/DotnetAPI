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
    [HttpPost("Post")]
    public IActionResult Post(PostToAddDto post)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Posts([UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated]) VALUES( '" + User.FindFirst("UserId")?.Value +
                "', '" + post.PostTitle + 
                "', '" + post.PostContent + 
                "', GETDATE(), GETDATE() )";
                if(_dapper.ExecuteSql(sql)) return StatusCode(201);
                throw new Exception("Failed to create new post");
    }
    [HttpPut("Post")]
    public IActionResult EditPost(PostToEditDto post)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Posts
            SET [PostTitle] = '" + post.PostTitle +
            "', [PostContent] = '" + post.PostContent +
            @"', [PostUpdated] = GETDATE()
            WHERE PostId = " + post.PostId.ToString() + 
            "AND UserId = " + User.FindFirst("UserId")?.Value;
        if(_dapper.ExecuteSql(sql)) return StatusCode(200);
        throw new Exception("Failed to edit new post");
    }
    } 
}