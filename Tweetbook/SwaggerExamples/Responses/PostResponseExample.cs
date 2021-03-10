using Swashbuckle.AspNetCore.Filters;
using Tweetbook.Contracts.Responses;

namespace Tweetbook.SwaggerExamples.Responses
{
    public class PostResponseExample : IExamplesProvider<PostResponse>
    {
        public PostResponse GetExamples()
        {
            return new PostResponse
            {
                Name = "Example name"
            };
        }
    }
}