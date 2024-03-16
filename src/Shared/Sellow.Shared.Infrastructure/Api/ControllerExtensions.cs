using Microsoft.AspNetCore.Http;

namespace Sellow.Shared.Infrastructure.Api;

public static class ControllerExtensions
{
    public static string GetActionFullUrlPath(this HttpRequest httpRequest)
        => $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}{httpRequest.Path}";
}