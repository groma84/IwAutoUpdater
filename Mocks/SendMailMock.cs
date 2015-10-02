using System;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using IwAutoUpdater.DAL.EMails.Contracts;

namespace Mocks
{
    public class SendMailMock : ISendMail
    {
        public SendMailMock()
        {
        }

        void ISendMail.Send(MailData mailData, string pickupDirectory)
        {
            throw new NotImplementedException();
        }

        void ISendMail.Send(MailData mailData, AddressUsernamePassword mailSettings)
        {
            throw new NotImplementedException();
        }
    }
}