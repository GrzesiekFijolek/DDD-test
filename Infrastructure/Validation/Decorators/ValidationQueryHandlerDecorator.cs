using Application.Common.CQRS;
using FluentValidation;
using FluentValidation.Results;

namespace Infrastructure.Validation.Decorators;

internal sealed class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _queryHandler;
    private readonly IEnumerable<IValidator<TQuery>> _validators;

    public ValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler, IEnumerable<IValidator<TQuery>> validators)
    {
        _queryHandler = queryHandler;
        _validators = validators;
    }

    public async Task<TResult> HandleAsync(TQuery query)
    {
        var failures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(query);
            if (!result.IsValid)
                failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await _queryHandler.HandleAsync(query);
    }
}
