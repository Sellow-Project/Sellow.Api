using Sellow.Shared.Infrastructure;
using Sellow.Shared.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args).AddLogging();

builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseInfrastructure();

app.Run();