using Microsoft.Extensions.DependencyInjection;

namespace Sellow.Modules.EmailSending.Core.EmailClient.Sendgrid;

internal static class Extensions
{
    public static IServiceCollection AddSendgrid(this IServiceCollection services)
        => services
            .AddScoped<SendgridClient>()
            .AddOptions<SendgridOptions>().BindConfiguration("Sendgrid")
            .Services;
}