    using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class NotificationType
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
