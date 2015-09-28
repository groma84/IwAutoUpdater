using IwAutoUpdater.CrossCutting.Configuration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.EMails.Contracts
{
    public interface ISendMail
    {
        void Send(MailData mailData, AddressUsernamePassword mailSettings);
        void Send(MailData mailData, string pickupDirectory);
    }
}
