using System.Net;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

public sealed record Username
{
    public string Value { get; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is < 3 or > 25 || value.Contains(' '))
        {
            throw new InvalidUsernameException(value);
        }

        Value = value;
    }

    public static implicit operator Username(string username) => new(username);

    public static implicit operator string(Username username) => username.Value;
}

public sealed class InvalidUsernameException : PresentableException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.BadRequest;
    public override string ErrorCode => "invalid_username";

    public InvalidUsernameException(string username) : base($"Username '{username}' is invalid.")
    {
    }
}