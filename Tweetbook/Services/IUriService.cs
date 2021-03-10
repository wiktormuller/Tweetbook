using System;
using Tweetbook.Contracts.Requests.Queries;

namespace Tweetbook.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);
        Uri GetAllPostsUri(PaginationQuery pagination);
    }
}