using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public partial class Address
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Settlement { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string HouseNumber { get; set; } = null!;

        public string? ApartmentNumber { get; set; }

        public int ZipCode { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime LastEdited { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); 
    }

}
