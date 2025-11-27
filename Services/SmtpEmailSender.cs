using System.Net;
using System.Net.Mail;


namespace TheDetectiveQuestTracker.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _enableSsl;
        private readonly string _username;
        private readonly string _password;
        private readonly string _from;

        public SmtpEmailSender(
            string host,
            int port,
            bool enableSsl,
            string username,
            string password,
            string from)
        {
            _host = host;
            _port = port;
            _enableSsl = enableSsl;
            _username = username;
            _password = password;
            _from = from;
        }

        public void Send(string to, string subject, string body)
        {
            using var message = new MailMessage(_from, to, subject, body);

            // Om du vill ha radbrytningar snyggt i vissa mejlklienter:
            message.IsBodyHtml = false;

            using var client = new SmtpClient(_host, _port)
            {
                EnableSsl = _enableSsl,
                Credentials = new NetworkCredential(_username, _password)
            };

            client.Send(message);
        }
    }
}

