namespace Dzone.Mobile.Interfaces;

public interface ILocalTokenService
{
    Task<ErrorOr<bool>> IsTokenValidAsync();
    Task<ErrorOr<string>> GetJwtTokenAsync();
    Task<ErrorOr<UserTbl>> GetUserInformationAsync();
    //===============================================================
    Task<ErrorOr<bool>> DeleteJwtTokenAsync();
    Task<ErrorOr<bool>> UpsertJwtTokenAsync(LoginResponce loginResponce);
}
