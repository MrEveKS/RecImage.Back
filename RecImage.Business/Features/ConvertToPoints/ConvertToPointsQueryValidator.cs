using FluentValidation;

namespace RecImage.Business.Features.ConvertToPoints;

public sealed class ConvertToPointsQueryValidator : AbstractValidator<ConvertToPointsQuery>
{
    public ConvertToPointsQueryValidator()
    {
        RuleFor(x => x.Image)
            .NotNull();

        RuleFor(x => x.Size)
            .GreaterThan(0);

        RuleFor(x => x.ColorStep)
            .IsInEnum();
    }
}