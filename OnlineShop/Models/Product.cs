using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public double Weight { get; set; }

    public int StockQuantity { get; set; }

    public string? CategoryId { get; set; }

    public string? BrandId { get; set; }

    public string? RepresentativeId { get; set; }

    public string Image { get; set; } = null!;

    public DateTime LastEdited { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Representative? Representative { get; set; }
}