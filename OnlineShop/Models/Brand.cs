using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class Brand
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    public string Name { get; set; } = null!;

    public DateTime LastEdited { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}