using System;
using System.Collections.Generic;

namespace Dzone.Models.Shered;

public partial class Ordar
{
    public string Id { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public string UserId { get; set; } = null!;

    public string LocationId { get; set; } = null!;

    public double Total { get; set; }

    public bool? IsPaid { get; set; }

    public string? CaptainId { get; set; }

    public string? PaymentLink { get; set; }

    public DateTime? EndTime { get; set; }

    public string Notes { get; set; } = null!;

    public virtual AspNetUser? Captain { get; set; }

    public virtual ICollection<OrderContent> OrderContents { get; set; } = new List<OrderContent>();

    public virtual AspNetUser User { get; set; } = null!;
}
