﻿using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace WebStorage
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email,string subject,string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Administration", "admin@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25);
                await client.AuthenticateAsync("admin@gmail.com", "password"); 
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
