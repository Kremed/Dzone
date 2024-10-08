global using Dzone.Models.Shered;
global using Dzone.Shared.Dtos;
global using System.ComponentModel.DataAnnotations;

namespace Dzone.Shared.Contracts.Products;

public class StartDataDto
{
    public List<Category> categories { get; set; } = new();
    public List<Product> products { get; set; } = new();
}
