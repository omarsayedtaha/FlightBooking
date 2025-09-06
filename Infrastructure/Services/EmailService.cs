using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

public class EmailService : IMailService
{
    private readonly IConfiguration config;
    private readonly IOptions<MailSettings> mailsettings;

    public EmailService(IOptions<MailSettings> mailsettings)
    {
        this.mailsettings = mailsettings;
    }
    public void SendEmail(string Recepient, string Body)
    {
        var mail = new MailMessage();
        mail.From = new MailAddress(mailsettings.Value.Email);

        mail.To.Add(Recepient);

        mail.Subject = "Otp";

        mail.Body = Body;

        var Host = mailsettings.Value.Host;
        var Port = mailsettings.Value.Port;
        var smtp = new SmtpClient(Host, Port);
        smtp.Credentials = new NetworkCredential(mailsettings.Value.Email, mailsettings.Value.Password);
        smtp.EnableSsl = true;

        smtp.Send(mail);


    }
}