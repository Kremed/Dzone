namespace Dzone.Backend.ServicesInterfaces
{
    public interface IUserOtpService
    {
        Task<int> CreateEmailOtpCode(string userID);
        Task<bool> IsValidEmailOtpCode(int code, string userID);
    }
}
