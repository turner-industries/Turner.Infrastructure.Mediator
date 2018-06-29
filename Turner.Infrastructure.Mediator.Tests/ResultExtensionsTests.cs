using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class ResultExtensionsTests : BaseUnitTest
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
    }

    public class FakeRequest : IRequest
    {
        public string Foo { get; set; }
    }
}
