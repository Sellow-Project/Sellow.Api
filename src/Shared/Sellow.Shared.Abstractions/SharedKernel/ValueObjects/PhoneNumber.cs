using System.Net;
using System.Text.RegularExpressions;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

public sealed record PhoneNumber
{
    private static readonly Regex PolishPhoneNumberRegex = new(@"^\+48 [0-9]{3}-[0-9]{3}-[0-9]{3}$");

    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidPhoneNumberException(value);
        }

        if (PolishPhoneNumberRegex.IsMatch(value) is false)
        {
            throw new InvalidPhoneNumberException(value);
        }

        Value = value;
    }

    public static implicit operator PhoneNumber(string phoneNumber) => new(phoneNumber);

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
}

public sealed class InvalidPhoneNumberException : PresentableException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.BadRequest;
    public override string ErrorCode => "invalid_phone_number";

    public InvalidPhoneNumberException(string phoneNumber) : base($"Phone number '{phoneNumber}' is invalid.")
    {
    }
}