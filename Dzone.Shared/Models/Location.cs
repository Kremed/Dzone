using System;
using System.Collections.Generic;

namespace Dzone.Models.Shered;

public partial class Location
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Discrption { get; set; } = null!;

    public string Latitude { get; set; } = null!;

    public string Longitude { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Ordar> Ordars { get; set; } = new List<Ordar>();

    public virtual AspNetUser User { get; set; } = null!;
}
