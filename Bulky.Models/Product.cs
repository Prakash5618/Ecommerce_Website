﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Display(Name = "Price for 3+")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Price for 6+")]
        public double Price50 { get; set; }
        [Required]
        [Display(Name = "Price for 10+")]
        public double Price100 { get; set; }   
        
        public int CategoryId { get; set; } 
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
