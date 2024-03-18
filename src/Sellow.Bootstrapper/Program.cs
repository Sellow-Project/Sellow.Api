using Sellow.Modules.Auth.Api;
using Sellow.Modules.EmailSending.Api;
using Sellow.Shared.Infrastructure;
using Sellow.Shared.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args).AddLogging();

builder.Services
    .AddInfrastructure()
    .AddAuthModule()
    .AddEmailSendingModule();

var app = builder.Build();

app.MapControllers();

app
    .UseInfrastructure()
    .UseAuthModule();

app.Run();