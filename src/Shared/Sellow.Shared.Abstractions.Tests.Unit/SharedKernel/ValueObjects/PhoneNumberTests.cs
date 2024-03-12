using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Shared.Abstractions.Tests.Unit.SharedKernel.ValueObjects;

public sealed class PhoneNumberTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abcabc")]
    [InlineData("123123123")]
    [InlineData("123-123-123")]
    [InlineData(" 123-123-123 ")]
    [InlineData(" 123-123- 123")]
    [InlineData("+48123123123")]
    [InlineData("+48 123123123")]
    [InlineData("+48 123-123123")]
    internal void should_not_allow_to_create_invalid_phone_number(string phoneNumber)
        => Assert.Throws<InvalidPhoneNumberException>(() => new PhoneNumber(phoneNumber));

    [Fact]
    internal void should_create_a_valid_phone_number()
        => Assert.Equal("+48 123-123-123", new PhoneNumber("+48 123-123-123").Value);
}