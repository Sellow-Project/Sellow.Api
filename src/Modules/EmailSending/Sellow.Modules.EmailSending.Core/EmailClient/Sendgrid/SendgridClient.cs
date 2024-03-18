using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Sellow.Modules.EmailSending.Core.EmailClient.Sendgrid;

internal sealed class SendgridClient
{
    private readonly ISendGridClient _sendGridClient;
    private readonly ILogger<SendgridClient> _logger;

    public SendgridClient(ILogger<SendgridClient> logger, IOptions<SendgridOptions> sendgridOptions)
    {
        _sendGridClient = new SendGridClient(sendgridOptions.Value.ApiKey);
        _logger = logger;
    }

    public async Task SendEmail(SendGridMessage email, CancellationToken cancellationToken)
    {
        await _sendGridClient.SendEmailAsync(email, cancellationToken);

        _logger.LogInformation("Email '{@Email}' has been sent", email.Serialize());
    }
}