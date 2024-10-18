using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class OrderProduct
{
    public string OrderId { get; set; } = Guid.NewGuid().ToString();

    public string ProductId { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}