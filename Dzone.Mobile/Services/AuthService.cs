using ErrorOr;

namespace Dzone.Mobile.Services;

internal class AuthService(IRestClient client) : IAuthService
{
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

    public async Task<string> LoginAsync(LoginContract contract)
    {
        try
        {
            var request = new RestRequest("Authentication/login", Method.Post);

            request.AddBody(contract);

            var response = await client.ExecuteAsync(request);

            return "";
        }
        catch (Exception)
        {
            return "";
        }
    }
}