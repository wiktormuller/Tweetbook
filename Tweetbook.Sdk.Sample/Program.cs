using System;
using System.Threading.Tasks;
using Refit;
using Tweetbook.Contracts.Requests;

namespace Tweetbook.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;
            
            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
            var tweetbookApi = RestService.For<ITweetbookApi>("https://localhost:5001", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Password123!"
            });
            
            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Password123!"
            });

            cachedToken = loginResponse.Content.Token;

            var allPosts = await tweetbookApi.GetAllAsync();

            var createdPost = await tweetbookApi.CreateAsync(new CreatePostRequest
            {
                Name = "This is created by SDK."
            });

            var retrievedPost = await tweetbookApi.GetAsync(createdPost.Content.Id);

            var updatedPost = await tweetbookApi.UpdateAsync(createdPost.Content.Id, new UpdatePostRequest
            {
                Name = "This is updated by SDK"
            });

            var deletePost = await tweetbookApi.DeleteAsync(createdPost.Content.Id);
        }
    }
}