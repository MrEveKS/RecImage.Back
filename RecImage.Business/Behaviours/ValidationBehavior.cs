using FluentValidation;
using Mapster;
using MediatR;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Behaviours;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);

        var validates = _validators
            .Select(s => s.ValidateAsync(context, cancellationToken))
            .ToArray();

        await Task.WhenAll(validates);

        var failures = validates
            .Select(v => v.Result)
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .Select(e => e.ErrorMessage)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        return Result.Bad(string.Join(". ", failures)).Adapt<TResponse>();
    }
}