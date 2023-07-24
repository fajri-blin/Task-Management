using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Task_Management.Contract.Data;

namespace Task_Management.Utilities.Handler
{
    public class EmailHandler : IEmailHandler
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmailAddress;
        private readonly string _smtpPassword;

        public EmailHandler(string smtpServer, int smtpPort, string fromEmailAddress, string smtpPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _fromEmailAddress = fromEmailAddress;
            _smtpPassword = smtpPassword;
        }

        public void SendEmail(string toEmail, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", _fromEmailAddress));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlMessage;
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);

            // Jika menggunakan Gmail, Anda perlu mengatur kredensial yang valid
            client.Authenticate(_fromEmailAddress, _smtpPassword);

            client.Send(message);
            client.Disconnect(true);
        }
    }
}
