using FluentValidation;
using Tweetbook.Contracts.Requests;

namespace Tweetbook.Validators
{
    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$")
                .Must(x => x.Contains(" "));
        }
    }
}