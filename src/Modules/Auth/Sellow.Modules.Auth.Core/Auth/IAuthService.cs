namespace Sellow.Modules.Auth.Core.Auth;

internal interface IAuthService
{
    Task CreateUser(ExternalAuthUser user, CancellationToken cancellationToken);
    Task ActivateUser(Guid id, CancellationToken cancellationToken);
}