using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailsettings;
        private readonly IConfiguration _configuration;

        public EmailSender(IOptions<EmailSettings> emailSettings, IConfiguration configuration)
        {
             _emailsettings = emailSettings.Value;
            _configuration = configuration;

        }

        public async Task Execute( string email, string subject, string message)
        {
            try
            {
                string ToEmail = string.IsNullOrEmpty(email) ? _emailsettings.ToEmail : email;
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(_emailsettings.UsernameEmail, "Book Shopping Application")
                };
                mailMessage.To.Add(ToEmail);
                mailMessage.CC.Add(_emailsettings.CCEmail);
                mailMessage.Subject = "Shopping App : " + subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                using (SmtpClient smtpClient = new SmtpClient(_emailsettings.PrimaryDomain, _emailsettings.PrimaryPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailsettings.UsernameEmail, _emailsettings.UsernamePassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }
            }

            catch(Exception error)
            {
                string err = error.Message;

            }
        }


        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Execute(email, subject, htmlMessage).Wait();
            return Task.FromResult(0);
        }
    }
}
