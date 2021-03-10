using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Tweetbook.Contracts.Requests;
using Tweetbook.Contracts.Responses;

namespace Tweetbook.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface ITweetbookApi
    {
        [Get("/api/v1/posts")]
        Task<ApiResponse<List<PostResponse>>> GetAllAsync();
        
        [Get("/api/v1/posts/{postId}")]
        Task<ApiResponse<PostResponse>> GetAsync(Guid postId);
        
        [Post("/api/v1/posts")]
        Task<ApiResponse<PostResponse>> CreateAsync([Body] CreatePostRequest createPostRequest);
        
        [Put("/api/v1/posts")]
        Task<ApiResponse<PostResponse>> UpdateAsync(Guid postId, [Body] UpdatePostRequest updatePostRequest);
        
        [Delete("/api/v1/posts")]
        Task<ApiResponse<string>> DeleteAsync(Guid postId);
    }
}