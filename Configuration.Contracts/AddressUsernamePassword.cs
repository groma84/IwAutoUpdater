namespace IwAutoUpdater.CrossCutting.Configuration.Contracts
{
    public class AddressUsernamePassword
    {
        public string Address;
        public string Username;
        public string Password;

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var typed = (AddressUsernamePassword)obj;
            if (typed == null)
            {
                return false;
            }

            return string.Equals(Address, typed.Address) 
                && string.Equals(Username, typed.Username)
                && string.Equals(Password, typed.Password);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode() ^ Username.GetHashCode() ^ Password.GetHashCode();

        }
    }
}