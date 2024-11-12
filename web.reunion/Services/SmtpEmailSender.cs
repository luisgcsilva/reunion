using MailKit.Net.Smtp;
using MimeKit;
using web.reunion.Interfaces;

namespace web.reunion.Services
{
    public class SmtpEmailSender(IConfiguration configuration) : ISenderEmail
    {
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string toEmail, string subject, MimeEntity message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Agenda Formação", _configuration["Smtp:Username"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = message;

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]!), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
