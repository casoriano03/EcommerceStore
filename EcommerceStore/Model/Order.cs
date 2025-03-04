﻿using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Cart> OrderItems { get; set; } = new List<Cart>();
        public string Status { get; set; } = "Pending";
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Precision(18,2)]
        public decimal Total { get; set; }
    }
}
