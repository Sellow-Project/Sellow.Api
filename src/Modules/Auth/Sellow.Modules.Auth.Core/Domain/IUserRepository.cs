namespace Sellow.Modules.Auth.Core.Domain;

internal interface IUserRepository
{
    Task<bool> IsUserUnique(User user, CancellationToken cancellationToken);
    Task Add(User user, CancellationToken cancellationToken);
}