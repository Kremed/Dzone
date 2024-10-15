using Newtonsoft.Json;

namespace Dzone.Mobile.Services;

public class AuthService(IRestClient client) : IAuthService
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

    public async Task<ErrorOr<LoginResponce>> LoginAsync(LoginContract contract)
    {
        try
        {
            var request = new RestRequest("Authentication/login", Method.Post);

            request.AddBody(contract);
           
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<LoginResponce>(response.Content!)!;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Error.Unauthorized(description: response.Content!.ToString());
            }
            else
            {
                return Error.Failure(description: response.Content!.ToString());
            }
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }
}