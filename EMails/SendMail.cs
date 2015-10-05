using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.EMails.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.EMails
{
    public class SendMail : ISendMail
    {
        void ISendMail.Send(MailData mailData, string pickupDirectory)
        {
            var smtpClient = new SmtpClient();
            smtpClient.PickupDirectoryLocation = pickupDirectory;

            SendMails(mailData, smtpClient);
        }

        void ISendMail.Send(MailData mailData, AddressUsernamePassword mailSettings)
        {
            var hostAndPort = mailSettings.Address.Split(new[] { ':' });

            var smtpClient = new SmtpClient();
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
