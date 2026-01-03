using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Domain.Exceptions;
using Restaurant.UI.Dialog;
using Restaurant.UI.Exceptions;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Restaurant.UI
{
    internal static class Extensions
    {
        public const string EMAIL_PATTERN = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        public static IEmailOptions LoadOptions(this IEmailOptions options)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt";
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s;
                    int i = 1;
                    while ((s = sr.ReadLine()) != null)
                    {
                        switch (i)
                        {
                            case 1:
                                options.Email = s;
                                break;
                            case 2:
                                options.SmtpClient = s;
                                break;
                            case 3:
                                options.SmtpPort = Convert.ToInt32(s);
                                break;
                            case 4:
                                options.Login = s.DecodeString();
                                break;
                            case 5:
                                options.Password = s.DecodeString();
                                break;
                            default:
                                break;
                        }
                        
                        i++;
                    }
                }

                if (options.IsEmpty())
                {
                    throw new RestaurantClientException("Nie wszystkie pola są wypełnione", typeof(Extensions).FullName, "LoadOptions");
                }
            }
            catch
            {
                throw;
            }

            return options;
        }

        public static IEmailOptions SaveOptions(this IEmailOptions options)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt";  // ścieżka do pliku settings.txt
            
            using (StreamWriter sw = File.CreateText(path)) // tworzenie i zapisywanie wartości do pliku settings.txt
            {
                sw.WriteLine(options.Email);
                sw.WriteLine(options.SmtpClient);
                sw.WriteLine(options.SmtpPort);
                sw.WriteLine(options.Login.EncodeString());
                sw.WriteLine(options.Password.EncodeString());
            }

            return options;
        }

        public static string EncodeString(this string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeString(this string text)
        {
            byte[] data = Convert.FromBase64String(text);
            return ASCIIEncoding.ASCII.GetString(data);
        }

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
            else if(exception.GetType() == typeof(RestaurantServerException))
            {
                MessageBox.Show(exception.Message,
                        ((RestaurantServerException)exception).Context,
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
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

            if (!Regex.Match(email, Extensions.EMAIL_PATTERN).Success)
            {
                return false;
            }

            return true;
        }
    }
}
