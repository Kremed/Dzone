using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Dzone.Mobile.Services;

public class SqliteTokenService : ILocalTokenService
{
    //Configration
    //===============================================================
    public ISqliteSevice SqliteSevice { get; }
    public ISQLiteAsyncConnection DbConnection { get; set; }

    public SqliteTokenService(ISqliteSevice SqliteSevice)
    {
        this.SqliteSevice = SqliteSevice;
        DbConnection = SqliteSevice.CreatConnection();
    }

   
    //Logic =>
    //===============================================================
    public async Task<ErrorOr<bool>> DeleteJwtTokenAsync()
    {
        try
        {
            await DbConnection.DeleteAllAsync<UserTbl>();

            return true;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<UserTbl>> GetUserInformationAsync()
    {
        try
        {
            var UserInformation = await DbConnection.Table<UserTbl>().FirstOrDefaultAsync();

            if (UserInformation is null ||
                string.IsNullOrEmpty(UserInformation.token))
                return Error.NotFound(description: "لايوجد اي بيانات للمستخدم, الرجاء تسجيل الدخول");

            return UserInformation;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> GetJwtTokenAsync()
    {
        try
        {
            var UserInformation = await GetUserInformationAsync();

            if (UserInformation.IsError ||
               string.IsNullOrEmpty(UserInformation.Value.token))
            {
                return Error.NotFound(description: "لايوجد اي بيانات للمستخدم, الرجاء تسجيل الدخول");
            }

            return UserInformation.Value.token;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> IsTokenValidAsync()
    {
        try
        {
            var UserInformation = await GetUserInformationAsync();

            if (UserInformation.IsError ||
                UserInformation.Value.expairyDate < DateTime.Now)
            {
                return false;
            }

            return true;

        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> UpsertJwtTokenAsync(LoginResponce loginResponce)
    {
        try
        {
            var user = ExtractClaimsFromToken(loginResponce.token);

            if (user.IsError)
                return user.Errors.FirstOrDefault();

            await DeleteJwtTokenAsync();

            await DbConnection.InsertAsync(user.Value);

            return true;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    private ErrorOr<UserTbl> ExtractClaimsFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            UserTbl user = new()
            {
                token = token,
                createdDate = jwtToken.ValidFrom,
                expairyDate = jwtToken.ValidTo,
                name = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value!,
                email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value!,
                phone = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.MobilePhone)?.Value!,
                globalID = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value!,
                role = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value!,
            };

            return user;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
}
