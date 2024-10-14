using Dzone.Models.Shered;
using Newtonsoft.Json;

namespace Dzone.Mobile.Services;

internal class ProductsService(IRestClient client) : IProductsService
{
    public async Task<ErrorOr<List<Category>>> GetStartData()
    {
        try
        {
            var request = new RestRequest("Products/startData", Method.Get);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<Category>>(response.Content!.ToString())!;
            }
            else
                return Error.Failure(response.StatusCode.ToString(), response.Content!);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<Category>> GetCategoryById(string categoryId)
    {
        try
        {
            var request = new RestRequest("Products/getCategoryById", Method.Get);

            request.AddBody(categoryId);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Category>(response.Content!.ToString())!;
            }
            else
                return Error.Failure(response.StatusCode.ToString(), response.Content!);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<Product>> GetProductById(string productId)
    {
        try
        {
            var request = new RestRequest("Products/getProductById", Method.Get);

            request.AddBody(productId);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Product>(response.Content!.ToString())!;
            }
            else
                return Error.Failure(response.StatusCode.ToString(), response.Content!);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }
    }
}