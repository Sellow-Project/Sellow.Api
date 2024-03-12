using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Sellow.Shared.Infrastructure.Api;

internal static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
        => services
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Sellow"
                });

                var xmlFiles = Directory
                    .GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                    .ToList();

                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
            })
            .AddFluentValidationRulesToSwagger();

    public static IApplicationBuilder UseSwagger_(this IApplicationBuilder app)
        => app
            .UseSwagger()
            .UseSwaggerUI()
            .UseReDoc();
}