using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class ErrorExtensionsTests : BaseUnitTest
    {
        [Test]
        public async Task AsResponse_ReturnsResponseWithError()
        {
            // Arrange
            var error = new Error { ErrorMessage = "Error.", PropertyName = "Property" };

            // Act
            var result = await error.AsResponseAsync();

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }
        
        [Test]
        public async Task AsResponseWithResult_ReturnsResponseWithError()
        {
            // Arrange
            var error = new Error { ErrorMessage = "Error.", PropertyName = "Property" };

            // Act
            var result = await error.AsResponseAsync<string>();

            // Assert
            result.Data.ShouldBe(null);
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].PropertyName.ShouldBe("Property");
            result.Errors[0].ErrorMessage.ShouldBe("Error.");
        }
    }
}
