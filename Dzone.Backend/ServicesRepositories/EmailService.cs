
using System.Net.Mail;

namespace Dzone.Backend.ServicesRepositories
{
    public class EmailService(SmtpClient smtpClient, MailMessage mailMessage) : IEmailService
    {
       
        public async Task<bool> SendConfirmationEmail(string otp, string email)
        {
            try
            {

                mailMessage.Body = otp;

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendResetPasswordEmail(string otp, string email)
        {
            try
            {

                mailMessage.Body = $"The temporary code to recover your account password is : <h4>{otp}</h4>";

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
