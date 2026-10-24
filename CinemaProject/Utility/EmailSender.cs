
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace CinemaProject.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage)
        {
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    "anazizo2006@gmail.com",
                    "aitlvhyqodicxftw"
                )
            };

            using var message = new MailMessage(
                "anazizo2006@gmail.com",
                email,
                subject,
                htmlMessage
            )
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
        }
    }
}
