using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Payment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public double AmountOfMoney { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
