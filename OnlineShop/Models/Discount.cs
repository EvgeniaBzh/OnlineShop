using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Discount
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string DiscountCode { get; set; } = null!;

    public double Dimensions { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}