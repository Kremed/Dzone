namespace Dzone.Shared.Contracts.Ordars;

public class CreateOrderContract
{
    [Required]
    public string locationId { get; set; }
    public string notes { get; set; }
    public List<OrderContentDto> orderContents { get; set; }
}
