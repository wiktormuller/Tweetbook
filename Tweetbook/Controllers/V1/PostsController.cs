using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.Requests;
using Tweetbook.Contracts.Requests.Queries;
using Tweetbook.Contracts.Responses;
using Tweetbook.Contracts.V1;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Filters;
using Tweetbook.Helpers;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1.PostsController
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUriService _uriService;
        public PostsController(IPostService postService, IUriService uriService)
        {
            _postService = postService;
            _uriService = uriService;
        }
        
        /// <summary>
        /// Returns all the posts in the system.
        /// </summary>
        /// <response code="200">Returns all the posts in the system.</response>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        [Cache(600)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
            
            var posts = await _postService.GetPostsAsync(paginationFilter);
            
            var postResponse = posts.Select(post => new PostResponse
            {
                Id = post.Id,
                Name = post.Name
            }).ToList();
            
            if (paginationFilter is null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<PostResponse>(postResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, postResponse);
            
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        [Cache(600)]
        public async Task<IActionResult> Get([FromRoute]Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post is null)
            {
                return NotFound();
            }

            var postResponse = new PostResponse
            {
                Id = post.Id,
                Name = post.Name
            };

            return Ok(new Response<PostResponse>(postResponse));
        }

        /// <summary>
        /// Creates posts in the system.
        /// </summary>
        /// <response code="201">Creates posts in the system.</response>
        /// <response code="400">Unable to create the tag due to validation error.</response>
        [HttpPost(ApiRoutes.Posts.Create)]
        [ProducesResponseType(typeof(PostResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody]CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId()
            };

            var created = await _postService.CreatePostAsync(post);
            if (!created)
            {
                return BadRequest(new ErrorResponse{Errors = new List<ErrorModel>{new ErrorModel{Message = "Unable to create post"}}});
            }
            
            var locationUri = _uriService.GetPostUri(post.Id.ToString());
            
            var postResponse = new PostResponse
            {
                Id = post.Id,
                Name = post.Name
            };
            return Created(locationUri, new Response<PostResponse>(postResponse));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromQuery]Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new {Error = "You do not own this post."});
            }

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);

            var postResponse = new PostResponse
            {
                Id = post.Id,
                Name = post.Name
            };
            
            if (updated)
                return Ok(new Response<PostResponse>(postResponse));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        [Authorize(Policy = "MustWorkForGoogle")]
        public async Task<IActionResult> Delete([FromQuery] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new {Error = "You do not own this post."});
            }
            
            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}