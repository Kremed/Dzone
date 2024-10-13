namespace Dzone.Mobile.Services;

public class AuthService(IRestClient client) : IAuthService
{
    public async Task<bool> LoginAsync(LoginContract contract)
    {
        try
        {
            var request = new RestRequest("Authentication/login", Method.Post);

            request.AddBody(contract);

            var response = await client.ExecuteAsync(request);

            return true;
        }
        catch (Exception)
        {
            return true;
        }
    }

    public async Task<ErrorOr<bool>> CreateUserAsync(RegisterContract contract)
    {
        try
        {
            var request = new RestRequest("Authentication/register", Method.Post);

            request.AddBody(contract);

            var response = await client.ExecuteAsync(request);

            return response.IsSuccessStatusCode ? true :
                Error.Failure(response.StatusCode.ToString(), response.Content!);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return Error.Failure(response.StatusCode.ToString(), response.Content!);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> ConfirmEmail(ConfirmEmailContract contract)
    {
        try
        {
            var request = new RestRequest("Authentication/confirmEmail", Method.Post);

            request.AddBody(contract);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return Error.Failure(response.StatusCode.ToString(), response.Content!);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> ForgotPassword(ForgetPasswordRequest contract)
    {
        try
        {
            var request = new RestRequest("Authentication/forgotPassword", Method.Post);

            request.AddBody(contract);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return Error.Failure(response.StatusCode.ToString(), response.Content!);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> RestPassword(RestPasswordRequest contract)
    {
        try
        {
            var request = new RestRequest("Authentication/restPassword", Method.Post);

            request.AddBody(contract);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return Error.Failure(response.StatusCode.ToString(), response.Content!);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }
}