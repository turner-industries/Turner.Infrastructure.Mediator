using System;
using System.Threading.Tasks;
using FluentValidation;
using NUnit.Framework;
using Shouldly;
using SimpleInjector;
using Turner.Infrastructure.Mediator.Decorators;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class ValidationHandlerTests : BaseUnitTest
    {
        [SetUp]
        public void Setup()
        {
            Container.Register<IValidator<ValidatedRequest>, ValidatedRequestValidator>();
        }

        [Test]
        public async Task HasDoNotValidateAttribute_DoesNotValidateRequest()
        {
            // Act
            var response = await Mediator.HandleAsync(new NonValidatedRequest());

            // Assert
            response.HasErrors.ShouldBe(false);
        }

        [Test]
        public void NoValidatorExists_ThrowsError()
        {
            // Act
            Action result = () => Mediator.HandleAsync(new NoValidatorRequest()).Wait();

            // Assert
            result.ShouldThrow<ActivationException>();
        }

        [Test]
        public async Task Handle_WithoutDoNotValidateAttribute_ValidatesRequest()
        {
            // Act
            var response = await Mediator.HandleAsync(new ValidatedRequest());

            // Assert
            response.HasErrors.ShouldBe(true);
            response.Errors[0].ErrorMessage.ShouldBe("'Name' should not be empty.");
        }
    }

    [DoNotValidate]
    public class NonValidatedRequest : IRequest
    {
    }

    public class NonValidatedRequestHandler : IRequestHandler<NonValidatedRequest>
    {
        public Task<Response> HandleAsync(NonValidatedRequest request)
        {
            return request.SuccessAsync();
        }
    }

    public class NoValidatorRequest : IRequest
    {
    }

    public class ValidatedRequest : IRequest
    {
        public string Name { get; set; }
    }

    public class ValidatedRequestHandler : IRequestHandler<ValidatedRequest>
    {
        public Task<Response> HandleAsync(ValidatedRequest request)
        {
            return request.SuccessAsync();
        }
    }

    public class ValidatedRequestValidator : AbstractValidator<ValidatedRequest>
    {
        public ValidatedRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
