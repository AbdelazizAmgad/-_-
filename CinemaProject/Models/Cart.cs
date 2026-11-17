using CinemaProject.ViewModels;
using System;
using System.Collections.Generic;

namespace CinemaProject.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}