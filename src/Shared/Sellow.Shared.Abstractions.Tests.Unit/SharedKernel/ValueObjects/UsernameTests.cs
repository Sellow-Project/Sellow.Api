using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Shared.Abstractions.Tests.Unit.SharedKernel.ValueObjects;

public sealed class UsernameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("            ")]
    [InlineData("ab")]
    [InlineData(" ab ")]
    [InlineData("   ab")]
    [InlineData("a a  aa   a")]
    [InlineData("waaaaaaaaaaaaaaaaay-to-long-username")]
    internal void should_not_allow_to_create_invalid_username(string username)
        => Assert.Throws<InvalidUsernameException>(() => new Username(username));

    [Theory]
    [InlineData("abc")]
    [InlineData("a.b.c-d!")]
    [InlineData("qweqweqweqweqweqweqweqweq")]
    internal void should_create_a_valid_username(string username)
        => Assert.Equal(username, new Username(username).Value);
}