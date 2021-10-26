using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ModelValidation.Validation_Attributes;

namespace ModelValidation.Models
{
    public class Product
    {

        public string ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [ValidateCategoryId]
        public byte CategoryId { get; set; }

        [Required]
        [ValidatePrice]
        
        public decimal Price { get; set; }

        [Required]
        [ValidateQuantityAvailable]
        public int QuantityAvailable { get; set; }

        public List<string> ImageUrls { get; set; }


        public bool ValidateQuantityAvailable()
        {
            if (QuantityAvailable >= 0) return true;
            else return false;
        }
        public bool ValidatePrice()
        {
            if (Price > 0) return true;
            else return false;
        }
        public bool ValidateCategoryId()
        {
            if (CategoryId > 0) return true;
            else return false;

        }

    }
}
