namespace Dzone.Backend.ServicesInterfaces
{
    public interface IUserOtpService
    {
        /// <summary>
        /// Generates a random OTP (One-Time Password) for email verification and stores it in the database 
        /// with relevant information, such as the user ID, creation time, and OTP type.
        /// </summary>
        /// <param name="userID">The unique identifier of the user for whom the OTP is being generated.</param>
        /// <returns>
        /// Returns the generated OTP code as an integer, which can be used for email verification.
        /// </returns>
        /// <remarks>
        /// This method generates a random OTP, creates a new <see cref="Otp"/> entity, and saves it in the database.
        /// The OTP is associated with the provided <paramref name="userID"/> and is used for email verification, with the type set to "Email."
        /// </remarks>
        Task<int> CreateEmailOtpCode(string userID);
        /// <summary>
        /// Validates the provided OTP (One-Time Password) for email verification against the most recently generated OTP
        /// for the specified user. It checks the OTP code and ensures it has not expired (within the last 10 minutes).
        /// </summary>
        /// <param name="code">The OTP code entered by the user for email verification.</param>
        /// <param name="userID">The unique identifier of the user requesting the email verification.</param>
        /// <returns>
        /// Returns a <see cref="bool"/> indicating whether the provided OTP is valid. 
        /// It returns <c>true</c> if the OTP matches the most recent one and is within the allowed time window; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method queries the database for the latest OTP associated with the user for email verification,
        /// checks if the OTP matches the provided code, and ensures that the OTP is still valid (created within the last 10 minutes).
        /// If the OTP is expired, missing, or doesn't match, the method returns <c>false</c>. Any exceptions will also result in a <c>false</c> return value.
        /// </remarks>
        Task<bool> IsValidEmailOtpCode(int code, string userID);

        /// <summary>
        /// Generates a random OTP (One-Time Password) for resetting the password of a user 
        /// and stores it in the database with relevant information, such as the user ID, 
        /// creation time, and OTP type.
        /// </summary>
        /// <param name="userID">The unique identifier of the user requesting the password reset.</param>
        /// <returns>
        /// Returns the generated OTP code as an integer, which can be used for password reset verification.
        /// </returns>
        /// <remarks>
        /// This method generates a random OTP, creates a new <see cref="Otp"/> entity, and saves it in the database.
        /// The OTP is associated with the provided <paramref name="userID"/> and has a type of "Reset Password."
        /// </remarks>
        Task<int> CreateRestPasswordOtpCode(string userID);

        /// <summary>
        /// Validates the provided OTP (One-Time Password) for password reset against the most recently generated OTP
        /// for the specified user. It checks the OTP code and ensures it has not expired (within the last 10 minutes).
        /// </summary>
        /// <param name="code">The OTP code entered by the user for password reset.</param>
        /// <param name="userID">The unique identifier of the user requesting the password reset.</param>
        /// <returns>
        /// Returns a <see cref="bool"/> indicating whether the provided OTP is valid. 
        /// It returns <c>true</c> if the OTP matches the most recent one and is within the allowed time window; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method queries the database for the latest OTP associated with the user, verifies that the OTP matches, 
        /// and ensures the OTP has not expired (within a 10-minute validity window).
        /// Any exceptions encountered during the operation will result in a <c>false</c> return value.
        /// </remarks>
        Task<bool> IsValidRestPasswordOtpCode(int code, string userID);
    }
}
