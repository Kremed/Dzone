using System;
using System.Collections.Generic;

namespace Dzone.Models.Shered;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Discrption { get; set; } = null!;

    public double SellPrice { get; set; }

    public double BuyPrice { get; set; }

    public int Quantity { get; set; }

    public string? Colors { get; set; }

    public string? Sizes { get; set; }

    public string FirstImage { get; set; } = null!;

    public string? SecondImage { get; set; }

    public string? ThirdImage { get; set; }

    public int ViewsCount { get; set; }

    public int LiksCount { get; set; }

    public string CategoryId { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderContent> OrderContents { get; set; } = new List<OrderContent>();
}
