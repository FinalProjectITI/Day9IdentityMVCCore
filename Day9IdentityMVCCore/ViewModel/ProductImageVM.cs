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
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is Required")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Quntity is Required")]
        public int Quntity { get; set; }
        public double? Discount { get; set; }
        [Required(ErrorMessage = "CategoryId is Required")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "TypeId is Required")]
        public int TypeId { get; set; }
        [Required(ErrorMessage = "SeasonId is Required")]
        public int SeasonId { get; set; }
        public bool ShowInHome { get; set; }
        public List<IFormFile> ImagePath { get; set; }
        [Required(ErrorMessage = "Product is Required")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
