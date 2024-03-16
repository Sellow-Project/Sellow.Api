using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Sellow.Modules.Auth.Contracts.IntegrationEvents;
using Sellow.Modules.Auth.Core.Auth;
using Sellow.Modules.Auth.Core.Domain;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Modules.Auth.Core.Features;

internal sealed record CreateUser(string Email, string Username, string Password) : IRequest<Guid>;

internal sealed class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail address is required.")
            .EmailAddress().WithMessage("Invalid e-mail address.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be 3 - 25 characters long.")
            .MaximumLength(25).WithMessage("Username must be 3 - 25 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}

internal sealed class CreateUserHandler : IRequestHandler<CreateUser, Guid>
{
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public CreateUserHandler(ILogger<CreateUserHandler> logger, IUserRepository userRepository,
        IAuthService authService, IMediator mediator)
    {
        _logger = logger;
        _userRepository = userRepository;
        _authService = authService;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = new User(request.Email, request.Username);

        var isUserUnique = await _userRepository.IsUserUnique(user, cancellationToken);
        if (isUserUnique is false)
        {
            throw new UserAlreadyExistsException();
        }

        await _userRepository.Add(user, cancellationToken);

        _logger.LogInformation("User '{@User}' was saved to the database", user);

        await CreateUserInExternalAuthSystem(request, user, cancellationToken);

        var createdUser = new UserCreated(user.Id, user.Email, user.Username);
        await _mediator.Publish(createdUser, cancellationToken);

        return user.Id;
    }

    private async Task CreateUserInExternalAuthSystem(CreateUser request, User user,
        CancellationToken cancellationToken)
    {
        try
        {
            var externalAuthUser = new ExternalAuthUser(user.Id, user.Email, user.Username, request.Password);
            await _authService.CreateUser(externalAuthUser, cancellationToken);
        }
        catch
        {
            await _userRepository.Delete(user, cancellationToken);
            throw;
        }
    }
}

internal sealed class UserAlreadyExistsException : PresentableException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.Conflict;
    public override string ErrorCode => "user_already_exists";

    public UserAlreadyExistsException() : base("User with given credentials already exists.")
    {
    }
}