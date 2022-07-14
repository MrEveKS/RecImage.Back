using FluentValidation;

namespace RecImage.Business.Features.ContactMessage;

public sealed class ContactMessageQueryValidator : AbstractValidator<ContactMessageQuery>
{
    public ContactMessageQueryValidator()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.UserMessage)
            .NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty();
    }
}