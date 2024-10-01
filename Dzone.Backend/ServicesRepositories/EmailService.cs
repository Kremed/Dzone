
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

        //public Task<bool> SendMarketingEmail(string email)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
