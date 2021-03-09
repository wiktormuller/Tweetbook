using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetTagsAsync();
    }
}