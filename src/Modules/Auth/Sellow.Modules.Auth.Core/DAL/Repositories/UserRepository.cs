using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Auth.Core.Domain;

namespace Sellow.Modules.Auth.Core.DAL.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserUnique(User user, CancellationToken cancellationToken)
    {
        Expression<Func<User, bool>> userUniquenessPredicate =
            x => x.Email == user.Email || x.Username == user.Username;

        return await _context.Users.FirstOrDefaultAsync(userUniquenessPredicate, cancellationToken) is null;
    }

    public async Task Add(User user, CancellationToken cancellationToken)
    {
        await _context.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(User user, CancellationToken cancellationToken)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}