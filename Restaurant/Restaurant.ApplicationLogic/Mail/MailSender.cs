using Restaurant.Domain.Exceptions;
using Restaurant.ApplicationLogic.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.ApplicationLogic.Mail
{
    internal sealed class MailSender : IMailSender
    {
        private readonly IEmailOptions _options;

        public MailSender(IEmailOptions options)
        {
            _options = options;
        }

        public async Task SendAsync(Email email, EmailMessage emailMessage)
        {
            var isEmpty = _options.IsEmpty();

            if (isEmpty)
            {
                throw new RestaurantServerException("There is no configured emai", typeof(MailSender).FullName, "SendMail");
            }

            var mail = new MailMessage(_options.Email, email.Value);
            using (var smtp = new SmtpClient(_options.SmtpClient, _options.SmtpPort))
            {
                mail.Subject = emailMessage.Subject;
                mail.Body = emailMessage.Body;
                smtp.Timeout = 5200;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_options.Login, _options.Password);
                smtp.EnableSsl = true;

                var cancellationTokenSource = new CancellationTokenSource(smtp.Timeout);
                cancellationTokenSource.CancelAfter(smtp.Timeout);
                var cancellationToken = cancellationTokenSource.Token;

                try
                {
                    var source = new CancellationTokenSource();
                    source.CancelAfter(TimeSpan.FromSeconds(5));
                    var token = source.Token;
                    var tcs = new TaskCompletionSource<bool>();
                    cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
                    var task = smtp.SendMailAsync(mail);
                    await Task.WhenAny(task, tcs.Task);
                    token.ThrowIfCancellationRequested();
                    await task;
                }
                catch
                {
                    throw new RestaurantServerException("Mail can't be sent. Probably invalid settings, please fill properly", typeof(MailSender).FullName, "SendMail");
                }
                
            }
        }
    }
}
