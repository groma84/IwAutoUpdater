namespace SFW.Contracts
{
    public class BlackboardEntry
    {
        public object Content;

        public BlackboardEntry()
        {

        }

        public BlackboardEntry(object content)
            : this()
        {
            Content = content;
        }
    }
}