using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class TagService : ITagService
    {
        public Task<List<Tag>> GetTagsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}