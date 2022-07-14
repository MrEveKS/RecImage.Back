using FluentValidation;

namespace RecImage.Business.Features.ConvertToPointsById;

public sealed class ConvertToPointsByIdQueryValidator : AbstractValidator<ConvertToPointsByIdQuery>
{
    public ConvertToPointsByIdQueryValidator()
    {
        RuleFor(x => x.ImageId)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Size)
            .GreaterThan(0);

        RuleFor(x => x.ColorStep)
            .IsInEnum();
    }
}