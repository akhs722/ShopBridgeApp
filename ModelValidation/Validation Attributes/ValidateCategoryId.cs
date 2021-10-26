using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ModelValidation.Models;

namespace ModelValidation.Validation_Attributes
{
    class ValidateCategoryId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = validationContext.ObjectInstance as Product;
            if (!product.ValidateCategoryId())
                return new ValidationResult("Invalid value for CategoryId");

            return ValidationResult.Success;
        }
    }
}
