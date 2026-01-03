namespace Restaurant.API.DTO
{
    public class SettingsDto
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SmtpClient { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
