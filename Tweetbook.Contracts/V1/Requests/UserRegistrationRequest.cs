using System.ComponentModel.DataAnnotations;

namespace Tweetbook.Contracts.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}