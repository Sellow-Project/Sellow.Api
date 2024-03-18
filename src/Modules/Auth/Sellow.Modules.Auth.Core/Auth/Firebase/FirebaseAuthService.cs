using System.Net;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Modules.Auth.Core.Auth.Firebase;

internal sealed class FirebaseAuthService : IAuthService
{
    private readonly ILogger<FirebaseAuthService> _logger;

    public FirebaseAuthService(ILogger<FirebaseAuthService> logger)
    {
        _logger = logger;
    }

    public async Task CreateUser(ExternalAuthUser user, CancellationToken cancellationToken)
    {
        await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
        {
            Uid = user.Id.ToString(),
            Email = user.Email,
            DisplayName = user.Username,
            Password = user.Password,
            Disabled = true,
            EmailVerified = false
        }, cancellationToken);

        _logger.LogInformation("Firebase user '{@User}' has been created", user);
    }

    public async Task ActivateUser(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var firebaseUser = await FirebaseAuth.DefaultInstance.GetUserAsync(id.ToString(), cancellationToken);
            if (firebaseUser.EmailVerified)
            {
                throw new FirebaseUserCannotBeActivatedException();
            }

            await FirebaseAuth.DefaultInstance.UpdateUserAsync(new UserRecordArgs
            {
                Uid = id.ToString(),
                EmailVerified = true,
                Disabled = false
            }, cancellationToken);

            _logger.LogInformation("Firebase user '{Id}' has been activated", id);
        }
        catch (FirebaseAuthException firebaseAuthException)
        {
            if (firebaseAuthException.AuthErrorCode == AuthErrorCode.UserNotFound)
            {
                throw new FirebaseUserCannotBeActivatedException();
            }

            throw;
        }
    }
}

internal sealed class FirebaseUserCannotBeActivatedException : PresentableException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.UnprocessableEntity;
    public override string ErrorCode => "user_cannot_be_activated";

    public FirebaseUserCannotBeActivatedException() : base(
        "User cannot be activated, user was not found or is already active.")
    {
    }
}