using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sellow.Shared.Infrastructure.Api;

internal static class VersioningExtensions
{
    public static IServiceCollection AddVersioning(this IServiceCollection services)
        => services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .Services;
}