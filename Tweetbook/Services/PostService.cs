using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _dbContext;
        
        public PostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<List<Post>> GetPostsAsync(GetAllPostsFilter getAllPostsFilter = null, PaginationFilter paginationFilter = null)
        {
            var queryable = _dbContext.Posts.AsQueryable();
            if (paginationFilter is null)
            {
                return await queryable.ToListAsync();   
            }

            queryable = AddFiltersOnQuery(getAllPostsFilter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dbContext.Posts.Where(x => x.Id == postId).FirstOrDefaultAsync();
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            var created = await _dbContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _dbContext.Posts.Update(postToUpdate);
            var updated = await _dbContext.SaveChangesAsync();
            
            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post == null)
            {
                return false;
            }
            
            _dbContext.Posts.Remove(post);
            var deleted = await _dbContext.SaveChangesAsync();
            
            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dbContext.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == postId);

            if (post is null)
            {
                return false;
            }

            if (post.UserId != userId)
            {
                return false;
            }

            return true;
        }
        
        private static IQueryable<Post> AddFiltersOnQuery(GetAllPostsFilter getAllPostsFilter, IQueryable<Post> queryable)
        {
            if (!string.IsNullOrEmpty(getAllPostsFilter?.UserId))
            {
                queryable = queryable.Where(x => x.UserId == getAllPostsFilter.UserId);
            }

            return queryable;
        }
    }
}