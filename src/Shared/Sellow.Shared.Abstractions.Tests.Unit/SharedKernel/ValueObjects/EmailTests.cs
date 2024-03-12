using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Shared.Abstractions.Tests.Unit.SharedKernel.ValueObjects;

public sealed class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("jankowalski")]
    [InlineData("jankowalski@")]
    [InlineData("jankowalski@email")]
    [InlineData("@email.com")]
    [InlineData(" jankowalski@email.com")]
    [InlineData("jankowalski@email.com ")]
    [InlineData(" jankowalski@email.com ")]
    internal void should_not_allow_to_create_invalid_email(string email)
        => Assert.Throws<InvalidEmailException>(() => new Email(email));

    [Fact]
    internal void should_create_a_valid_email()
        => Assert.Equal("jankowalski@email.com", new Email("jankowalski@email.com").Value);
}