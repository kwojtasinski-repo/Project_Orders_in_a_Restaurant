namespace Restaurant.Shared.DTO
{
    public class SettingsDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string SmtpClient { get; set; }
        public int SmtpPort { get; set; }
        public string Email { get; set; }
    }
}
