using System.Net;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

public sealed record Nip
{
    public string Value { get; }

    public Nip(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || IsNipValid(value) is false)
        {
            throw new InvalidNipException(value);
        }

        Value = value;
    }

    private static bool IsNipValid(string nip)
    {
        if (nip.Length != 10 || !nip.All(char.IsDigit))
        {
            return false;
        }

        int[] weights = [6, 5, 7, 2, 3, 4, 5, 6, 7, 0];
        var sum = nip.Zip(weights, (digit, weight) => (digit - '0') * weight).Sum();

        return sum % 11 == (nip[9] - '0');
    }

    public static implicit operator Nip(string nip) => new(nip);

    public static implicit operator string(Nip nip) => nip.Value;
}

public sealed class InvalidNipException : PresentableException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.BadRequest;
    public override string ErrorCode => "invalid_nip";

    public InvalidNipException(string nip) : base($"NIP '{nip}' is invalid.")
    {
    }
}