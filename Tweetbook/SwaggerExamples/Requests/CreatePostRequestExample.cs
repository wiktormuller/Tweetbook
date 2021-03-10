using Swashbuckle.AspNetCore.Filters;
using Tweetbook.Contracts.Requests;

namespace Tweetbook.SwaggerExamples.Requests
{
    public class CreatePostRequestExample : IExamplesProvider<CreatePostRequest>
    {
        public CreatePostRequest GetExamples()
        {
            return new CreatePostRequest
            {
                Name = "Some name"
            };
        }
    }
}