using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Infrastructure.Validation
{
    class RequestValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next , CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var failure = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if(failure.Count != 0)
            {
                throw new ValidationException(string.Join("\n", failure));
            }

            return next();
        }
    }
}
