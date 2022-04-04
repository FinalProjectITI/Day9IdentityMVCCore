using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdminDashBoard.Models;
using Microsoft.AspNetCore.Http;

namespace AdminDashBoard.ViewModel
{
    public class ProductImageVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Image is Required")]
        public IFormFile ImagePath { get; set; }
        [Required(ErrorMessage = "Product is Required")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
