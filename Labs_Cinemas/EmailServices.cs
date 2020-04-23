using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Labs_Cinemas
{
    public class EmailServices
    {
        public async Task SendEmailAsync(string email, string subject, string message) {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Labs Cinemas Administration", "stuntdenko@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
                Text = message
            };

            using (var client = new SmtpClient()) {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("stuntdenko@gmail.com", "denis2000");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
