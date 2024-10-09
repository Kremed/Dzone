namespace Dzone.Shared.Dtos;

public class OrderContentDto
{
    public int id { get; set; }
    public string productId { get; set; }
    public double price { get; set; }
    public string? note { get; set; }
    public int quantity { get; set; }
}