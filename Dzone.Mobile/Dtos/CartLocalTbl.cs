namespace Dzone.Mobile.Dtos;

public class CartLocalTbl
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string productId { get; set; }
    public string name { get; set; }
    public double price { get; set; }
    public int quantity { get; set; }
    public string? note { get; set; }
    public string image { get; set; }
}
