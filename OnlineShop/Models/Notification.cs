using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? TypeId { get; set; }

    public int Status { get; set; }

    public DateTime DeliveryDate { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual NotificationType? NotificationType { get; set; } = null!;
}
