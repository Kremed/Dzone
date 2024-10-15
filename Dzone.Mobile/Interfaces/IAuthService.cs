namespace Dzone.Mobile.Interfaces;

public interface IAuthService
{
    Task<ErrorOr<LoginResponce>> LoginAsync(LoginContract contract);

    Task<ErrorOr<bool>> CreateUserAsync(RegisterContract registerContract);

    Task<ErrorOr<bool>> ConfirmEmail(ConfirmEmailContract contract);

    Task<ErrorOr<bool>> ForgotPassword(ForgetPasswordRequest contract);

    Task<ErrorOr<bool>> RestPassword(RestPasswordRequest contract);
}