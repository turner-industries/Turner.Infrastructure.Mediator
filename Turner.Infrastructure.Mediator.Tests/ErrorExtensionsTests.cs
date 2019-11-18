using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class ErrorExtensionsTests : BaseUnitTest
    {
        [Test]
        public void AsResponse_ReturnsResponseWithError()
        {
            // Act
            var result = Error.AsResponse("Error.", "Property");

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }
        
        [Test]
        public void AsResponseWithResult_ReturnsResponseWithError()
        {
            // Act
            var result = Error.AsResponse<string>("Error.", "Property");

            // Assert
            result.Data.ShouldBe(null);
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }

        [Test]
        public async Task AsResponseAsync_ReturnsResponseWithError()
        {
            // Act
            var result = await Error.AsResponseAsync("Error.", "Property");

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }

        [Test]
        public async Task AsResponseAsyncWithResult_ReturnsResponseWithError()
        {
            // Act
            var result = await Error.AsResponseAsync<string>("Error.", "Property");

            // Assert
            result.Data.ShouldBe(null);
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }

        [Test]
        public void AsErrorResponse_ReturnsResponseWithError()
        {
            // Arrange
            var error = new Error { ErrorMessage = "Error.", PropertyName = "Property" };

            // Act
            var result = error.AsErrorResponse();

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }

        [Test]
        public void AsErrorResponseWithResult_ReturnsResponseWithError()
        {
            // Arrange
            var error = new Error { ErrorMessage = "Error.", PropertyName = "Property" };

            // Act
            var result = error.AsErrorResponse<string>();

            // Assert
            result.Data.ShouldBe(null);
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }

        [Test]
        public async Task AsErrorResponseAsync_ReturnsResponseWithError()
        {
            // Arrange
            var error = new Error { ErrorMessage = "Error.", PropertyName = "Property" };

            // Act
            var result = await error.AsErrorResponseAsync();

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }

        [Test]
        public async Task AsErrorResponseAsyncWithResult_ReturnsResponseWithError()
        {
            // Arrange
            var error = new Error { ErrorMessage = "Error.", PropertyName = "Property" };

            // Act
            var result = await error.AsErrorResponseAsync<string>();

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }

        [Test]
        public void AsErrorResponse_WithCollection_ReturnsResponseWithErrors()
        {
            // Arrange
            var errors = new[]
            {
                new Error("Error1.", "Property1"),
                new Error("Error2.", "Property2")
            };

            // Act
            var result = errors.AsErrorResponse();

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(2);
            result.Errors[0].PropertyName.ShouldBe("Property1");
            result.Errors[0].ErrorMessage.ShouldBe("Error1.");
            result.Errors[1].PropertyName.ShouldBe("Property2");
            result.Errors[1].ErrorMessage.ShouldBe("Error2.");
        }

        [Test]
        public void AsErrorResponseWithResult_WithCollection_ReturnsResponseWithErrors()
        {
            // Arrange
            var errors = new[]
            {
                new Error("Error1.", "Property1"),
                new Error("Error2.", "Property2")
            };

            // Act
            var result = errors.AsErrorResponse<string>();

            // Assert
            result.Data.ShouldBe(null);
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(2);
            result.Errors[0].PropertyName.ShouldBe("Property1");
            result.Errors[0].ErrorMessage.ShouldBe("Error1.");
            result.Errors[1].PropertyName.ShouldBe("Property2");
            result.Errors[1].ErrorMessage.ShouldBe("Error2.");
        }

        [Test]
        public async Task AsErrorResponseAsync_WithCollection_ReturnsResponseWithErrors()
        {
            // Arrange
            var errors = new[]
            {
                new Error("Error1.", "Property1"),
                new Error("Error2.", "Property2")
            };

            // Act
            var result = await errors.AsErrorResponseAsync();

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(2);
            result.Errors[0].PropertyName.ShouldBe("Property1");
            result.Errors[0].ErrorMessage.ShouldBe("Error1.");
            result.Errors[1].PropertyName.ShouldBe("Property2");
            result.Errors[1].ErrorMessage.ShouldBe("Error2.");
        }

        [Test]
        public async Task AsErrorResponseAsyncWithResult_WithCollection_ReturnsResponseWithErrors()
        {
            // Arrange
            var errors = new[]
            {
                new Error("Error1.", "Property1"),
                new Error("Error2.", "Property2")
            };

            // Act
            var result = await errors.AsErrorResponseAsync<string>();

            // Assert
            result.Data.ShouldBe(null);
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(2);
            result.Errors[0].PropertyName.ShouldBe("Property1");
            result.Errors[0].ErrorMessage.ShouldBe("Error1.");
            result.Errors[1].PropertyName.ShouldBe("Property2");
            result.Errors[1].ErrorMessage.ShouldBe("Error2.");
        }
    }
}
