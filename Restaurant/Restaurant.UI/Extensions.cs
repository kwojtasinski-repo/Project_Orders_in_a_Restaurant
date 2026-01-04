using Restaurant.UI.Dialog;
using Restaurant.UI.Exceptions;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Restaurant.UI
{
    internal static class Extensions
    {
        public const string EMAIL_PATTERN = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        public static void ShowDialog(string text, string caption, EventHandler eventHandler)
        {
            Form prompt = new DialogWindow(500, 150, caption, text, eventHandler);
            prompt.ShowDialog();
        }

        public static void MapToMessageBox(this Exception exception)
        {
            if (exception.GetType() == typeof(FileLoadException))
            {
                MessageBox.Show(exception.Message, "FileLoad",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (exception.GetType() == typeof(FileNotFoundException))
            {
                MessageBox.Show(exception.Message, "NotFoundFile",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (exception.GetType() == typeof(InvalidOperationException))
            {
                MessageBox.Show(exception.Message, "InvalidOperation",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            /*else if(exception.GetType() == typeof(RestaurantServerException))
            {
                MessageBox.Show(exception.Message,
                        ((RestaurantServerException)exception).Context,
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }*/
            else if (exception.GetType() == typeof(RestaurantClientException))
            {
                MessageBox.Show(exception.Message, 
                        ((RestaurantClientException) exception).Context,
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Coś poszło nie tak", "Error",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

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

            if (!Regex.Match(email, EMAIL_PATTERN).Success)
            {
                return false;
            }

            return true;
        }
    }
}
