using System;
using System.Collections.Generic;

namespace Dzone.Models.Shered;

public partial class OrderContent
{
    public int Id { get; set; }

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public double Price { get; set; }

    public string? Note { get; set; }

    public virtual Ordar Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
