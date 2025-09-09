using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Project_Manassas.Service;

public class SendGridEmailService: ISendGridService
{
    private readonly string? _apiKey;
    private readonly string? _fromEmail = "mitsumori@nashmanassas.org";
    private readonly string? _fromAppName = "Nash Manassas";

    public SendGridEmailService(IConfiguration configuration)
    {
        _apiKey = configuration["SendGrid:ApiKey"]; // store key in appSettings.json for now
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        var client = new SendGridClient(_apiKey);
        var fromEmail = new EmailAddress(_fromEmail, _fromAppName);
        var toRecipient = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(fromEmail, toRecipient, subject, "", htmlContent);

        await client.SendEmailAsync(msg);
        
        return true;
    }
    
}