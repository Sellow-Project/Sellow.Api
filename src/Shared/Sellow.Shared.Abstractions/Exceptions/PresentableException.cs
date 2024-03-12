using System.Net;

namespace Sellow.Shared.Abstractions.Exceptions;

public abstract class PresentableException : Exception
{
    public abstract HttpStatusCode HttpCode { get; }
    public abstract string ErrorCode { get; }

    protected PresentableException(string message) : base(message)
    {
    }
}