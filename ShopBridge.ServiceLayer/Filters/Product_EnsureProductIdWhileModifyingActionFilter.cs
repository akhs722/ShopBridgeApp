using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelValidation.Models;

namespace ShopBridge.ServiceLayer.Filters
{
    public class Product_EnsureProductIdWhileModifyingActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

          
            var product = context.ActionArguments["product"] as Product;
            if (string.IsNullOrWhiteSpace(product.ProductId))
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
