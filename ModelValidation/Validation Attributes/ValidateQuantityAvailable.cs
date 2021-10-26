using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ModelValidation.Models;

namespace ModelValidation.Validation_Attributes
{
    public class ValidateQuantityAvailable : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = validationContext.ObjectInstance as Product;
            if (!product.ValidateQuantityAvailable())
                return new ValidationResult("Invalid value for QuantityAvailable");

            return ValidationResult.Success;
        }
    }
}
