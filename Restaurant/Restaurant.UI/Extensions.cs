﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Restaurant.UI
{
    internal static class Extensions
    {
        public static Options LoadOptions(this Options options)
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
                    throw new InvalidOperationException("Dane są puste uzupełnij je");
                }
            }
            catch
            {
                throw;
            }

            return options;
        }

        public static Options SaveOptions(this Options options)
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
            byte[] data = System.Convert.FromBase64String(text);
            return System.Text.ASCIIEncoding.ASCII.GetString(data);
        }

        public static void MapToMessageBox(this Exception exception)
        {
            if (exception.GetType() == typeof(FileLoadException))
            {
                MessageBox.Show(exception.Message, "Błąd wczytywania pliku",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (exception.GetType() == typeof(FileNotFoundException))
            {
                MessageBox.Show(exception.Message, "Nie znaleziono pliku",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (exception.GetType() == typeof(InvalidOperationException))
            {
                MessageBox.Show(exception.Message, "Nie znaleziono pliku",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Coś poszło nie tak", "Nie znaleziono pliku",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}