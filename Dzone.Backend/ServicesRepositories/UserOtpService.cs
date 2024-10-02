
namespace Dzone.Backend.ServicesRepositories
{
    public class UserOtpService(DzoneDbContext context) : IUserOtpService
    {
        private int GetRandomOtp()
        {
            Random random = new Random();

            int RandomOtp = random.Next(10000, 99999);

            return RandomOtp;
        }

        public async Task<int> CreateEmailOtpCode(string userID)
        {
            int RandomOtp = GetRandomOtp();

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

        public async Task<int> CreateRestPasswordOtpCode(string userID)
        {
            int RandomOtp = GetRandomOtp();

            var userOtpCode = new Otp
            {
                Code = RandomOtp,
                CreateTime = DateTime.Now,
                Type = "Rest Password",
                UserId = userID,
            };

            await context.Otps.AddAsync(userOtpCode);

            await context.SaveChangesAsync();

            return RandomOtp;
        }
        public async Task<bool> IsValidRestPasswordOtpCode(int code, string userID)
        {
            try
            {
                var LastUserOtp = await context.Otps
                                  .Where(otp => otp.UserId == userID && otp.Type == "Rest Password")
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
