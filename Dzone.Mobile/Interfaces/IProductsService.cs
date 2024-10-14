using Dzone.Models.Shered;

namespace Dzone.Mobile.Interfaces;

public interface IProductsService
{
    Task<ErrorOr<List<Category>>> GetStartData();

    Task<ErrorOr<Category>> GetCategoryById(string categoryId);

    Task<ErrorOr<Product>> GetProductById(string categoryId);
}