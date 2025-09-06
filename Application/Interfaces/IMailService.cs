namespace Application.Interfaces
{
    public interface IMailService
    {
        public void SendEmail(string Resipient, string Body);
    }

}