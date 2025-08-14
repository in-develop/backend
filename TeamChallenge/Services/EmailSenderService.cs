using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using TeamChallenge.Helpers;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.SendEmailModels;

namespace TeamChallenge.Services
{
    public class EmailSenderService : IEmailSend
    {
        private readonly SenderModel _sender;
        public EmailSenderService(IOptions<SenderModel> options)
        {
            _sender = options.Value;
        }       

        public async Task SendEmail(string email, string body)
        {
            var message = new MailMessage();    
            message.From = new MailAddress(_sender.Email);
            message.To.Add(email);
            message.Subject = "Confirm your account!";
            
            message.Body = body;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(_sender.Host, _sender.Port)
            {
                Credentials = new NetworkCredential(_sender.Email, _sender.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }

    }
}
