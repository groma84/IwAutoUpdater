using System.Collections.Generic;

namespace IwAutoUpdater.DAL.EMails.Contracts
{
    public class MailData
    {
        public string From;
        public IEnumerable<string> Recipients;
        public string Subject;
        public string MessageBody; 
    }
}