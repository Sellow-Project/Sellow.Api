using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Auth.Core.DAL;

namespace Sellow.Modules.Auth.Core.Tests.Integration;

internal sealed class TestDatabase : IDisposable
{
    public AuthDbContext Context { get; }

    public TestDatabase()
    {
        var connectionString =
            $"Server=localhost;Port=5432;Database=Sellow-Auth-Tests-{Guid.NewGuid()};User Id=postgres;Password=postgres;";
        Context = new AuthDbContext(new DbContextOptionsBuilder<AuthDbContext>().UseNpgsql(connectionString).Options);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public async Task Init()
    {
        await Context.Database.EnsureCreatedAsync();
        await Seed();
    }

    private async Task Seed()
    {
        await Context.Database.ExecuteSqlRawAsync(
            "INSERT INTO \"Auth\".\"Users\"(\"Id\", \"Email\", \"Username\") VALUES ('39b17709-6fbf-4ac0-8c6d-b45698143f38'::uuid, 'jan@kowalski.com', 'jankowalski')");
    }

    public void Dispose() => Context.Database.EnsureDeleted();
}