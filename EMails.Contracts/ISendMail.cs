using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace IwAutoUpdater.DAL.EMails.Contracts
{
    public interface ISendMail
    {
        void Send(MailData mailData, AddressUsernamePassword mailSettings);
        void Send(MailData mailData, string pickupDirectory);
    }
}
