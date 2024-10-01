using System;
using System.Collections.Generic;

namespace Dzone.Models.Shered;

public partial class Otp
{
    public int Id { get; set; }

    public int Code { get; set; }

    public DateTime CreateTime { get; set; }

    public string UserId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
