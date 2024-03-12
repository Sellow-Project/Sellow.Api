using System.Net;
using System.Text.RegularExpressions;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

public sealed record PostalCode
{
    private static readonly Regex PolishPostalCodeRegex = new("^[0-9]{2}-[0-9]{3}$");

    public string Value { get; }

    public PostalCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || PolishPostalCodeRegex.IsMatch(value) is false)
        {
            throw new InvalidPostalCodeException(value);
        }

        Value = value;
    }

    public static implicit operator PostalCode(string postalCode) => new(postalCode);

    public static implicit operator string(PostalCode postalCode) => postalCode.Value;
}

public sealed class InvalidPostalCodeException : PresentableException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.BadRequest;
    public override string ErrorCode => "invalid_postal_code";

    public InvalidPostalCodeException(string postalCode) : base($"Postal code '{postalCode} is invalid.")
    {
    }
}