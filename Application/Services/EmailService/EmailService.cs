using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Application.Services.EmailService;

public class EmailService : IEmailService
{
    private string _mail;
    private string _pw;
    
    public EmailService(IConfiguration configuration)
    {
        _mail = configuration["SmtpCredentials:mail"]!;
        _pw = configuration["SmtpCredentials:pw"]!;
    }
    
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var mail = _mail;
        var pw = _pw;

        var client = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(mail, pw),
            Timeout = 20000
        };
        
        return client.SendMailAsync(new MailMessage(
            from: mail,
            to: email,
            subject,
            body: message
        ));
    }
}