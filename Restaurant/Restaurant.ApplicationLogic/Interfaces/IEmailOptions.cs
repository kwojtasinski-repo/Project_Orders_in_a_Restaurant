namespace Restaurant.ApplicationLogic.Interfaces
{
    public interface IEmailOptions
    {
        string Login { get; set; }
        string Password { get; set; }
        string SmtpClient { get; set; }
        int SmtpPort { get; set; }
        string Email { get; set; }

        bool IsEmpty();
    }
}
