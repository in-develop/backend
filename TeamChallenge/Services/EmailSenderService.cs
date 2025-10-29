using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TeamChallenge.Models.SendEmailModels;

namespace TeamChallenge.Services
{
    public class EmailSenderService(IOptions<SenderModel> options) : IEmailSend
    {
        private readonly SenderModel _sender = options.Value;

        public async Task SendEmail(string email, string body)
        {
            var message = new MailMessage();    
            message.From = new MailAddress(_sender.Email);
            message.To.Add(email);
            message.Subject = "Confirm your account!";
            
            message.Body = body;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(_sender.Host, _sender.Port);
            client.Credentials = new NetworkCredential(_sender.Email, _sender.Password);
            client.EnableSsl = true;

            await client.SendMailAsync(message);
        }
    }
}
