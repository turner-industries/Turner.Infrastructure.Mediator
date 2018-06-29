using FluentValidation;
using SimpleInjector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator.Decorators
{
    public class ValidationBaseHandler<TRequest, TResult> where TResult : Response, new()
    {
        private readonly Container _container;

        public ValidationBaseHandler(Container container)
        {
            _container = container;
        }

        public Task<TResult> HandleAsync(TRequest request, Func<Task<TResult>> processRequest)
        {
            var validator = _container.GetInstance<IValidator<TRequest>>();

            var validationResult = validator.Validate(request);
            if (validationResult.IsValid)
            {
                return processRequest();
            }

            var result = new TResult();
            result.Errors.AddRange(validationResult.Errors.Select(x => new Error
            {
                ErrorMessage = x.ErrorMessage,
                PropertyName = x.PropertyName
            }));
            return Task.FromResult(result);
        }
    }

    public class DoNotValidateAttribute : Attribute
    {
    }

    public class ValidationHandler<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
    {
        private readonly Func<IRequestHandler<TRequest>> _decorateeFactory;
        private readonly ValidationBaseHandler<TRequest, Response> _validationHandler;

        public ValidationHandler(Func<IRequestHandler<TRequest>> decorateeFactory,
            ValidationBaseHandler<TRequest, Response> validationHandler)
        {
            _decorateeFactory = decorateeFactory;
            _validationHandler = validationHandler;
        }

        public Task<Response> HandleAsync(TRequest request)
        {
            return _validationHandler.HandleAsync(request, () => _decorateeFactory().HandleAsync(request));
        }
    }

    public class ValidationHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
    {
        private readonly Func<IRequestHandler<TRequest, TResult>> _decorateeFactory;
        private readonly ValidationBaseHandler<TRequest, Response<TResult>> _validationHandler;

        public ValidationHandler(Func<IRequestHandler<TRequest, TResult>> decorateeFactory,
            ValidationBaseHandler<TRequest, Response<TResult>> validationHandler)
        {
            _decorateeFactory = decorateeFactory;
            _validationHandler = validationHandler;
        }

        public Task<Response<TResult>> HandleAsync(TRequest request)
        {
            return _validationHandler.HandleAsync(request, () => _decorateeFactory().HandleAsync(request));
        }
    }
}