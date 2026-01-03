using Restaurant.Domain.Exceptions;
using Restaurant.ApplicationLogic.Interfaces;
using System.Text.RegularExpressions;

namespace Restaurant.ApplicationLogic.Mail
{
    internal sealed class EmailOptions : IEmailOptions
    {
        private const string EMAIL_PATTERN = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        private string _email;

        public string Login { get; set; }
        public string Password { get; set; }
        public string SmtpClient { get; set; }
        public int SmtpPort { get; set; }
        public string Email { 
            get
            {
                return _email;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new RestaurantServerException("Email cannot be empty", typeof(EmailOptions).FullName, "OptionsEmail");
                }

                if (!Regex.Match(value, EMAIL_PATTERN).Success)
                {
                    throw new RestaurantServerException("Invalid Email", typeof(EmailOptions).FullName, "OptionsEmail");
                }

                _email = value;
            }
        }

        public bool IsEmpty()
        {
            var isEmptyLogin = string.IsNullOrWhiteSpace(Login);

            if(isEmptyLogin)
                return true;

            var isEmptyPassword = string.IsNullOrWhiteSpace(Password);

            if (isEmptyPassword)
                return true;

            var isEmptySmtpClient = string.IsNullOrWhiteSpace(SmtpClient);

            if (isEmptySmtpClient)
                return true;

            var isEmptySmtpPort = SmtpPort == 0;

            if (isEmptySmtpPort)
                return true;

            var isEmptyEmail = string.IsNullOrWhiteSpace(Email);

            if (isEmptyEmail)
                return true;

            return false;
        }
    }
}
