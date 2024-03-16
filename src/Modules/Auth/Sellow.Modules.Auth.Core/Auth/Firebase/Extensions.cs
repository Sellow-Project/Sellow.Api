using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sellow.Shared.Infrastructure.Options;

namespace Sellow.Modules.Auth.Core.Auth.Firebase;

internal static class Extensions
{
    public static IServiceCollection AddFirebaseAuth(this IServiceCollection services)
    {
        var firebaseOptions = services.GetOptions<FirebaseOptions>("Firebase");

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(firebaseOptions.ApiKeyFilePath),
            ProjectId = firebaseOptions.ProjectId
        });

        services.AddScoped<IAuthService, FirebaseAuthService>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = firebaseOptions.Authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = firebaseOptions.ValidIssuer,
                    ValidAudience = firebaseOptions.ValidAudience,
                    ValidateLifetime = true
                };
            });

        return services;
    }
}