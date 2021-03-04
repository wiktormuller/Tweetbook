using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.Requests;
using Tweetbook.Contracts.Responses;
using Tweetbook.Contracts.V1;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1.PostsController
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }
        
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetPosts());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute]Guid postId)
        {
            var post = _postService.GetPostById(postId);

            if (post is null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody]CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = postRequest.Id,
            };
            
            if (post.Id != Guid.Empty)
                post.Id = Guid.NewGuid();
            
            _postService.GetPosts().Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse
            {
                Id = post.Id
            };
            return Created(locationUrl, post);
        }
    }
}