namespace Dzone.Shared.Dtos
{
    public class ProductsDto
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Discrption { get; set; } = null!;

        public double SellPrice { get; set; }

        public int Quantity { get; set; }

        public string? Colors { get; set; }

        public string? Sizes { get; set; }

        public string FirstImage { get; set; } = null!;

        public string? SecondImage { get; set; }

        public string? ThirdImage { get; set; }

        public int ViewsCount { get; set; }

        public int LiksCount { get; set; }

        public string CategoryId { get; set; } = null!;
    }
}
