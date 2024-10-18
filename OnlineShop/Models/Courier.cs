using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Courier
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
