namespace Service;

using DTO;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
public class EmailService
{
    private readonly EmailCredentialsDTO emailCredentials;
    private SendGridClient client;
    private readonly ILogger<EmailService> logger;

    public EmailService(IOptions<EmailCredentialsDTO> emailOptions, ILogger<EmailService> logger)
    {
        this.emailCredentials = emailOptions.Value;
        if (string.IsNullOrEmpty(emailCredentials.Email) || string.IsNullOrEmpty(emailCredentials.Password) || string.IsNullOrEmpty(emailCredentials.ApiKey))
        {
            logger.LogError("EmailCredentials not configured properly in appsettings.json");
        }
    var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
    
    if (string.IsNullOrEmpty(apiKey))
    {
        logger.LogError("SENDGRID_API_KEY environment variable not configured");
        throw new InvalidOperationException("SendGrid API key not found in environment variables");
    }
    
    this.client = new SendGridClient(apiKey);
    this.logger = logger;

    }

    public async Task<bool> SendMail(string ToMail, string Body, string Subject)
    {
        try
        {

            var From = new EmailAddress(emailCredentials.Email, "FinanceSystem");
                
            var To = new EmailAddress(ToMail,"user");

            var mail = MailHelper.CreateSingleEmail(From, To, Subject, "", Body);
            var response = await client.SendEmailAsync(mail);
            var responseBody = await response.Body.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("SendGrid failed: {StatusCode}", response.StatusCode);
                logger.LogError(responseBody);
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Error While Email Sending "+ex);
            return false;
        }
    }
}