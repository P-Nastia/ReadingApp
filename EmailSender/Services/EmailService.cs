using MimeKit;
using System.Net.Mail;
using EmailSender.Constants;

namespace EmailSender.Services;

public static class EmailService
{
    public static void SendEmail(string to, string subject, string bodyText)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(EmailConfiguration.From));
        message.To.Add(new MailboxAddress(to));
        message.Subject = subject;

        var body = new TextPart("plain")
        {
            Text = bodyText
        };

        var multipart = new Multipart("mixed") { body };

        message.Body = multipart;

        using var client = new MailKit.Net.Smtp.SmtpClient();
        try
        {
            client.Connect(EmailConfiguration.SmtpServer, EmailConfiguration.Port, true);
            client.Authenticate(EmailConfiguration.UserName, EmailConfiguration.Password);
            client.Send(message);

        }
        catch (Exception ex)
        {

        }
        finally
        {
            client.Disconnect(true);
        }
    }
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
