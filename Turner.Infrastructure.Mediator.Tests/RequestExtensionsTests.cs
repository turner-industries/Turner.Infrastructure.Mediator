using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class RequestExtensionsTests : BaseUnitTest
    {
        [Test]
        public async Task AsResponse_ReturnsObjectAsDataInResponse()
        {
            // Arrange
            var request = new FakeRequest { Foo = "Bar" };

            // Act
            var result = await request.AsResponseAsync();

            // Assert
            result.Data.Foo.ShouldBe("Bar");
            result.HasErrors.ShouldBe(false);
            result.Errors.ShouldBeEmpty();
        }

        [Test]
        public async Task Success_ReturnsNewResponse()
        {
            // Arrange
            var request = new FakeRequest { Foo = "Bar" };

            // Act
            var result = await request.SuccessAsync();

            // Assert
            result.HasErrors.ShouldBe(false);
            result.Errors.ShouldBeEmpty();
        }

        [Test]
        public async Task HasError_ReturnsResponseWithError()
        {
            // Arrange
            var request = new FakeRequest { Foo = "Bar" };

            // Act
            var result = await request.HasErrorAsync("This is an error.");

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("This is an error.");
        }

        [Test]
        public async Task RequestWithResultError_ReturnsResponseWithError()
        {
            // Arrange
            var request = new FakeRequestWithResponse { Foo = "Bar" };

            // Act
            var result = await request.HasErrorAsync("This is an error.");

            // Assert
            result.HasErrors.ShouldBe(true);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("This is an error.");
        }
    }

    public class FakeRequest : IRequest
    {
        public string Foo { get; set; }
    }

    public class FakeRequestWithResponse : IRequest<string>
    {
        public string Foo { get; set; }
    }
}
