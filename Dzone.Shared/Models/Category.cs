using System;
using System.Collections.Generic;

namespace Dzone.Models.Shered;

public partial class Category
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string CoverImage { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
