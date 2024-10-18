using System;
using System.Collections.Generic;

namespace Shop.Models;

public partial class Feedback
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public int StarsNumber { get; set; }

    public string? Text { get; set; }

    public string? Photo { get; set; }

    public string? CustomerId { get; set; } 

    public string? ProductId { get; set; } 

    public virtual Customer? Customer { get; set; } = null!;

    public virtual Product? Product { get; set; } = null!; 
}
