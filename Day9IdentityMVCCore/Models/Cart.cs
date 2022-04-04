﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdminDashBoard.Models
{
    [Table("Cart")]
    public partial class Cart
    {
        public Cart()
        {
            Orders = new HashSet<Order>();
            Product_In_Carts = new HashSet<Product_In_Cart>();
        }

        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(450)]
        public string UserID { get; set; }
        public bool AddedToOrder { get; set; }

        [ForeignKey(nameof(UserID))]
        [InverseProperty(nameof(AspNetUser.Carts))]
        public virtual AspNetUser User { get; set; }
        [InverseProperty(nameof(Order.Cart))]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty(nameof(Product_In_Cart.Cart))]
        public virtual ICollection<Product_In_Cart> Product_In_Carts { get; set; }
    }
}