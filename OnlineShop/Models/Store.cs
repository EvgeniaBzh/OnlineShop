using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Store
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public string? RepresentativeId { get; set; }

    public int IsDeleted { get; set; }

    public virtual Representative? Representative { get; set; }
}
