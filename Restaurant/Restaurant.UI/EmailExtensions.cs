using System.Text.RegularExpressions;

namespace Restaurant.UI
{
    internal static partial class EmailExtensions
    {
        public const string EMAIL_PATTERN = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        public static string WithTwoDecimalPoints(this decimal value)
        {
            return string.Format("{0:0.00}", value);
        }

        public static bool ValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (!EmailRegex().IsMatch(email))
            {
                return false;
            }

            return true;
        }

        [GeneratedRegex(EMAIL_PATTERN)]
        private static partial Regex EmailRegex();
    }
}
