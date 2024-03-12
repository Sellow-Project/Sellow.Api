using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Shared.Abstractions.Tests.Unit.SharedKernel.ValueObjects;

public sealed class PostalCodeTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("qweqwe")]
    [InlineData("123qwe")]
    [InlineData(" 12 123")]
    [InlineData("12123")]
    [InlineData(" 1-213")]
    [InlineData("11- 222")]
    [InlineData(" 22-222 ")]
    [InlineData(" 22-222")]
    [InlineData("22-222 ")]
    internal void should_not_allow_to_create_invalid_username(string postalCode)
        => Assert.Throws<InvalidPostalCodeException>(() => new PostalCode(postalCode));

    [Fact]
    internal void should_create_a_valid_postal_code()
        => Assert.Equal("22-222", new PostalCode("22-222").Value);
}