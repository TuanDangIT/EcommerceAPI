using Ecommerce.Shared.Abstractions.Exceptions;
using Ecommerce.Shared.Abstractions.MediatR;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Behavior
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) 
            { 
                return await next(); 
            }
            Error[] errors = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
                .Distinct()
                .ToArray();
            if (errors.Any())
            {
                throw new Shared.Abstractions.Exceptions.ValidationException("One or more validation errors occured", errors);
            }
            return await next();
        }
    }
}
