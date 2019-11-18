using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class StringExtensionsTests : BaseUnitTest
    {
        private const string ErrorMessage = "Error.";

        [Test]
        public void AsError_ReturnsErrorWithMessageAndProperty()
        {
            // Act
            var result = ErrorMessage.AsError("Property");

            // Assert
            result.PropertyName.ShouldBe("Property");
            result.ErrorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void AsErrorResponse_ReturnsResponseWithError()
        {
            // Act
            var result = ErrorMessage.AsErrorResponse("Property");

            // Assert
            result.HasErrors.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void AsErrorResponseWithResult_ReturnsResponseWithError()
        {
            // Act
            var result = ErrorMessage.AsErrorResponse<string>("Property");

            // Assert
            result.HasErrors.ShouldBeTrue();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public async Task AsErrorResponseAsync_ReturnsResponseWithError()
        {
            // Act
            var result = await ErrorMessage.AsErrorResponseAsync("Property");

            // Assert
            result.HasErrors.ShouldBeTrue();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public async Task AsErrorResponseAsyncWithResult_ReturnsResponseWithError()
        {
            // Act
            var result = await ErrorMessage.AsErrorResponseAsync<string>("Property");

            // Assert
            result.HasErrors.ShouldBeTrue();
            result.Data.ShouldBeNull();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe(ErrorMessage);
        }
    }
}
