namespace Dzone.Backend.ServicesInterfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a confirmation email with a one-time password (OTP) to the specified email address.
        /// </summary>
        /// <param name="otp">The one-time password to include in the email for user confirmation or verification.</param>
        /// <param name="email">The recipient's email address where the confirmation email will be sent.</param>
        /// <returns>
        /// A Task that represents the asynchronous operation. The result is a boolean indicating the success (true) or failure (false) of sending the email.
        /// </returns>
        Task<bool> SendConfirmationEmail(string otp, string email);

        /// <summary>
        /// Sends a marketing email to the specified email address.
        /// </summary>
        /// <param name="email">The recipient's email address where the marketing email will be sent.</param>
        /// <returns>
        /// A Task that represents the asynchronous operation. The result is a boolean indicating the success (true) or failure (false) of sending the marketing email.
        /// </returns>
        //Task<bool> SendMarketingEmail(string email);
    }
}
