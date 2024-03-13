using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Shared.Abstractions.Tests.Unit.SharedKernel.ValueObjects;

public sealed class NipTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123123123123")]
    [InlineData("asdasd12312312")]
    [InlineData(" 12 123123  111")]
    [InlineData(" 1231231231 ")]
    internal void should_not_allow_to_create_invalid_phone_number(string nip)
        => Assert.Throws<InvalidNipException>(() => new Nip(nip));

    [Fact]
    internal void should_create_a_valid_nip_code()
        => Assert.Equal("5286633447", new Nip("5286633447"));
}