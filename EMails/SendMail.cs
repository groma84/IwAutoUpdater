using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.CrossCutting.Logging.Contracts;
using IwAutoUpdater.DAL.EMails.Contracts;
using System.Net;
using System.Net.Mail;

namespace IwAutoUpdater.DAL.EMails
{
    public class SendMail : ISendMail
    {
        private readonly ILogger _logger;

        public SendMail(ILogger logger)
        {
            _logger = logger;
        }

        void ISendMail.Send(MailData mailData, string pickupDirectory)
        {
            _logger.Debug("Sending Mail '{@Mail}' via Pickup Directory '{@PickupDir}'", mailData, pickupDirectory);

            var smtpClient = new SmtpClient();
            smtpClient.PickupDirectoryLocation = pickupDirectory;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

            SendMails(mailData, smtpClient);
        }

        void ISendMail.Send(MailData mailData, AddressUsernamePassword mailSettings)
        {
            _logger.Debug("Sending Mail '{@Mail}' via SMTP '{@Settings}'", mailData, mailSettings);

            var hostAndPort = mailSettings.Address.Split(new[] { ':' });

            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Host = hostAndPort[0];
            if (hostAndPort.Length > 1)
            {
                var port = int.Parse(hostAndPort[1]);
                smtpClient.Port = port;
            }
            if (mailSettings.UseDefaultCredentials)
            {
                smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                smtpClient.Credentials = new NetworkCredential(mailSettings.Username, mailSettings.Password);
            }

            SendMails(mailData, smtpClient);
        }

        private static void SendMails(MailData mailData, SmtpClient smtpClient)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(mailData.From),
                Subject = mailData.Subject,
                Body = mailData.MessageBody
            };

            foreach (var recipient in mailData.Recipients)
            {
                message.To.Add(recipient);
            }

            smtpClient.Send(message);
        }
    }
}
