namespace Sellow.Modules.EmailSending.Core.EmailClient.Sendgrid;

internal sealed class SendgridOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public EmailTemplates Templates { get; set; } = new();

    internal sealed class EmailTemplates
    {
        public UserActivationTemplate UserActivation { get; set; } = new();

        internal sealed class UserActivationTemplate
        {
            public string TemplateId { get; set; } = string.Empty;
            public string ActivationUrl { get; set; } = string.Empty;
        }
    }
}