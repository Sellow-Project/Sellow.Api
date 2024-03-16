namespace Sellow.Modules.Auth.Core.Auth.Firebase;

internal sealed class FirebaseOptions
{
    public string ApiKeyFilePath { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
}