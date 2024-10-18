using System;
using System.Collections.Generic;

namespace Shop.Models;

public enum OrderStatus
{
    Created,
    Processed,
    Sent,
    Delivered
}

public partial class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? CustomerId { get; set; }

    public decimal Price { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ShippingDate { get; set; }

    public string? ShippingAddressId { get; set; }

    public OrderStatus Status { get; set; }

    public string? PaymentId { get; set; }

    public string? CourierId { get; set; }

    public string DiscountCode { get; set; }

    public DateTime LastEditted { get; set; } = DateTime.UtcNow;

    public virtual Courier? Courier { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Payment? Payment { get; set; }

    public virtual Address? ShippingAddress { get; set; }
}