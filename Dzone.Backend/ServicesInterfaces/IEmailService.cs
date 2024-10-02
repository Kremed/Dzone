namespace Dzone.Backend.ServicesInterfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a confirmation email containing the provided OTP (One-Time Password) to the specified email address.
        /// </summary>
        /// <param name="otp">The OTP code to be included in the email body.</param>
        /// <param name="email">The recipient's email address to which the OTP will be sent.</param>
        /// <returns>
        /// Returns a <see cref="bool"/> indicating whether the email was successfully sent.
        /// Returns <c>true</c> if the email was sent successfully; otherwise, <c>false</c> if an exception occurred during the sending process.
        /// </returns>
        /// <remarks>
        /// This method uses the configured <see cref="SmtpClient"/> to send an email containing the OTP.
        /// If an exception occurs during the process, it catches the exception and returns <c>false</c>.
        /// </remarks>
        Task<bool> SendConfirmationEmail(string otp, string email);

        /// <summary>
        /// Sends a password reset email containing the provided OTP (One-Time Password) to the specified email address.
        /// The email includes a temporary password reset code in Arabic.
        /// </summary>
        /// <param name="otp">The OTP code to be included in the email body for password reset.</param>
        /// <param name="email">The recipient's email address to which the OTP will be sent.</param>
        /// <returns>
        /// Returns a <see cref="bool"/> indicating whether the email was successfully sent.
        /// Returns <c>true</c> if the email was sent successfully; otherwise, <c>false</c> if an exception occurred during the sending process.
        /// </returns>
        /// <remarks>
        /// This method uses the configured <see cref="SmtpClient"/> to send an email containing the OTP for password reset.
        /// The email body includes a formatted message in Arabic, informing the user of their temporary reset code. 
        /// If an exception occurs during the process, the method catches the exception and returns <c>false</c>.
        /// </remarks>
        Task<bool> SendResetPasswordEmail(string otp, string email);

    }
}
