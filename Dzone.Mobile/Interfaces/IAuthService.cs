namespace Dzone.Mobile.Interfaces;

public interface IAuthService
{
    Task<ErrorOr<bool>> CreateUserAsync(RegisterContract registerContract);

    Task<ErrorOr<bool>> LoginAsync(LoginContract contract);
}