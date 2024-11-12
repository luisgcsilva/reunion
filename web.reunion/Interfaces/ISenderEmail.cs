using MimeKit;

namespace web.reunion.Interfaces
{
    /// <summary>
    /// Interface de envio de email
    /// </summary>
    public interface ISenderEmail
    {
        Task SendEmailAsync(string toEmail, string subject, MimeEntity message);
    }
}
