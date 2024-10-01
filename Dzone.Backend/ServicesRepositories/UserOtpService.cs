
namespace Dzone.Backend.ServicesRepositories
{
    public class UserOtpService(DzoneDbContext context) : IUserOtpService
    {
        public async Task<int> CreateEmailOtpCode(string userID)
        {
            Random random = new Random();

            int RandomOtp = random.Next(10000, 99999);

            var userOtpCode = new Otp
            {
                Code = RandomOtp,
                CreateTime = DateTime.Now,
                Type = "Email",
                UserId = userID,
            };

            await context.Otps.AddAsync(userOtpCode);

            await context.SaveChangesAsync();

            return RandomOtp;
        }

        public async Task<bool> IsValidEmailOtpCode(int code, string userID)
        {
            try
            {
                var LastUserOtp = await context.Otps
                                  .Where(otp => otp.UserId == userID && otp.Type == "Email")
                                  .OrderByDescending(row => row.CreateTime)
                                  .FirstOrDefaultAsync();

                if (LastUserOtp is null ||
                    LastUserOtp.CreateTime < DateTime.Now.AddMinutes(-10) ||
                    LastUserOtp.Code != code)
                    
                    return false;

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
