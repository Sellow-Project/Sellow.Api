using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sellow.Modules.Auth.Core.Auth;
using Sellow.Modules.Auth.Core.DAL.Repositories;
using Sellow.Modules.Auth.Core.Features;

namespace Sellow.Modules.Auth.Core.Tests.Integration.Features;

public sealed class CreateUserTests : IDisposable
{
    private async Task Act(CreateUser command) => await _handler.Handle(command, default);

    [Fact]
    internal async Task should_not_allow_to_create_duplicated_user()
    {
        await _testDatabase.Init();
        var command = new CreateUser("jan@kowalski.pl", "jankowalski", "qwe123qwe!!");

        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => Act(command));
    }

    [Fact]
    internal async Task should_add_a_new_user_to_the_database()
    {
        await _testDatabase.Init();
        var command = new CreateUser("jan2@kowalski.pl", "jan2kowalski", "qwe123qwe!!");

        await Act(command);

        Assert.Equal(2, _testDatabase.Context.Users.Count());
    }

    [Fact]
    internal async Task should_remove_user_from_the_database_if_creation_in_external_auth_fails()
    {
        await _testDatabase.Init();
        _authServiceMock.CreateUser(Arg.Any<ExternalAuthUser>(), default).ThrowsAsync(new Exception());
        var command = new CreateUser("jan2@kowalski.pl", "jan2kowalski", "qwe123qwe!!");

        _ = await Record.ExceptionAsync(() => Act(command));

        Assert.Equal(1, _testDatabase.Context.Users.Count());
    }

    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly CreateUserHandler _handler;
    private readonly IAuthService _authServiceMock = Substitute.For<IAuthService>();

    public CreateUserTests()
    {
        _testDatabase = new TestDatabase();
        _handler = new CreateUserHandler(
            Substitute.For<ILogger<CreateUserHandler>>(),
            new UserRepository(_testDatabase.Context),
            _authServiceMock
        );
    }

    #endregion

    public void Dispose() => _testDatabase.Dispose();
}