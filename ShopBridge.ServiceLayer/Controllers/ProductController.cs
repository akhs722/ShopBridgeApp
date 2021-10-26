using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelValidation.Models;
using ShopBridge.BusinessLogicLayer;
using ShopBridge.ServiceLayer.Filters;

namespace ShopBridge.ServiceLayer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        Intermidiate inter = null;

        public ProductController()
        {
            inter = new Intermidiate();
        }


        public IActionResult Welcome()
        {

            string result = "Welcome to Product Admin API. Note: Please read the provided document to get the idea about the signature and structure of our endpoint.";
            return Ok(result);
        }

        #region AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            string status;
            try
            {
                //waiting to get the result from the BLL
                status = await inter.AddProduct(product);


                //Binding into DTO/View Model or response object

                ViewModels.Product p = new ViewModels.Product();

                if (status.Length >= 4 && status != "Exception caught at BLL" || status != "Exception caught at DAL")
                {
                    p.ProductId = status;
                    p.ProductName = product.ProductName;
                    p.CategoryId = product.CategoryId;
                    p.Price = product.Price;
                    p.QuantityAvailable = product.QuantityAvailable;
                    p.Status = 1.ToString();
                }
                else
                {
                    p.ProductId = "Not able to generate";
                    p.ProductName = product.ProductName;
                    p.CategoryId = product.CategoryId;
                    p.Price = product.Price;
                    p.QuantityAvailable = product.QuantityAvailable;
                    p.Status = status;
                }


                return Ok(p);
            }
            catch (Exception e)
            {
                return Ok("Exception caught at SL");
            }

        }
        #endregion

        #region ModifyProduct
        [HttpPost]
        [Product_EnsureProductIdWhileModifyingActionFilter]
        public async Task<IActionResult> ModifyProduct([FromBody] Product product)
        {
            string status;
            try
            {
                if (string.IsNullOrWhiteSpace(product.ProductId)) return Ok("Product Id is required");

                status = await inter.ModifyProduct(product);


                //Binding into DTO/View Model or response object
                if (status == "1")
                {
                    ViewModels.Product ap = new ViewModels.Product();

                    ap.ProductId = product.ProductId;
                    ap.ProductName = product.ProductName;
                    ap.Status = "1";
                    ap.Price = product.Price;
                    ap.QuantityAvailable = product.QuantityAvailable;
                    ap.CategoryId = product.CategoryId;
                    return Ok(ap);
                }
                else
                {
                    ViewModels.Product ap = new ViewModels.Product();

                    ap.ProductId = product.ProductId;
                    ap.ProductName = product.ProductName;
                    ap.Status = "Error unable to update, Note: Product Name should be unique";
                    ap.Price = product.Price;
                    ap.QuantityAvailable = product.QuantityAvailable;

                    ap.CategoryId = product.CategoryId;
                    return BadRequest(ap);
                }
            }
            catch (Exception)
            {
                return Ok(-99);
            }
        }
        #endregion

        #region DeleteProduct
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] string productId)
        {
            int status = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(productId)) return Ok("Product Id is required");

                if(productId.Length < 4) return Ok("Product Id is in Invalid format");
                
                status = await inter.DeleteProduct(productId);
                return Ok(status);
            }
            catch (Exception)
            {
                status = -99;
                return Ok(status);
            }

        }
        #endregion

        #region ViewAllProductsInInventory
        [HttpGet]
        public IActionResult GetAllProducts()
        {

            try
            {
                var products = inter.GetAllProducts();

                //ViewModelBinding or DTO
                //Converting the result Model we got from DAL into the response Model

                return Ok(products);
            }
            catch (Exception)
            {
                return Ok("Exception caught");
            }

        }
        #endregion


        #region GetAllCategories
        [HttpGet]
        public IActionResult GetAllCategories()
        {

            try
            {
                var categories = inter.GetAllCategories();

                //ViewModelBinding or DTO
                //Converting the result Model we got from DAL into the response Model

                return Ok(categories);
            }
            catch (Exception)
            {
                return Ok("Exception caught");
            }

        }
        #endregion



    }

}