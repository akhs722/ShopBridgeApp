using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ModelValidation.Models;
using ShoBridge.DataAccessLayer;
using ShoBridge.DataAccessLayer.Models;

namespace ShopBridge.BusinessLogicLayer
{

    //This is the intermediate class which will be used to perform below tasks -
    //1. Acting as a bridge between our channel that is the Service Layer and our Data Store that is the Data Access Layer.
    //2. Mapping request and response objects.
    //3. Adding extra logics required as per the business.
    
    public class Intermidiate
    {
        ShopBridgeRepository repo = null;
        public Intermidiate()
        {
            repo = new ShopBridgeRepository();
        }

        //Adding Product

        public async Task<string> AddProduct(Product product)
        {

            string status;
            try
            {
                
                //Mapping the request object as per the requirements of our DATA ACCESS LAYER
                ShoBridge.DataAccessLayer.Models.Products dataProduct =  new ShoBridge.DataAccessLayer.Models.Products();

                    //While adding the product, Product Id can be either null or any random value.
                    //ProductId will be generated at the data store while adding the product
                    dataProduct.ProductId = product.ProductId;
                    dataProduct.ProductName = product.ProductName;
                    dataProduct.CategoryId = product.CategoryId;
                    dataProduct.Price = product.Price;
                    dataProduct.QuantityAvailable = product.QuantityAvailable;
                
                status = await repo.AddProduct(dataProduct);
                if (status.Length == 4 && product.ImageUrls != null)
                {
                    await repo.AddProductImageUrl(status, product.ImageUrls);
                }

                return status;
            }
            catch (Exception e)
            {

                status = "Exception caught at BLL";
                Console.WriteLine(e.Message);
                return status;
            }

        }


        public async Task<string> ModifyProduct(Product product)
        {

            string status;
            try
            {
                //Checking if product Id is in valid format
                if (product.ProductId.Length < 4) return status = "Invalid Product Id";
                
                //Mapping the request object as per the requirements of our DATA ACCESS LAYER
                ShoBridge.DataAccessLayer.Models.Products dataProduct = new ShoBridge.DataAccessLayer.Models.Products();

                
                dataProduct.ProductId = product.ProductId;
                dataProduct.ProductName = product.ProductName;
                dataProduct.CategoryId = product.CategoryId;
                dataProduct.Price = product.Price;
                dataProduct.QuantityAvailable = product.QuantityAvailable;

                status = await repo.ModifyProduct(dataProduct);
                return status;
            }
            catch (Exception e)
            {

                status = "Exception caught at BLL";
                Console.WriteLine(e.Message);
                return status;
            }

        }


        public async Task<int> DeleteProduct(string productId)
        {
            int status = 0;
            try
            {
                
                status = await repo.DeleteProduct(productId);
                return status;
            }
            catch (Exception)
            {
                status = -99;
                return status;
            }
        }

        public List<Products> GetAllProducts()
        {
            List<Products> ProductList = null;
            try
            {
                ProductList = repo.GetAllProducts();
            }
            catch (Exception e)
            {
                ProductList = null;
                Console.WriteLine(e.Message);
            }
            return ProductList;
        }

        public List<Categories> GetAllCategories()
        {
            List<Categories> CategoryList = null;
            try
            {
                CategoryList = repo.GetAllCategories();
            }
            catch (Exception e)
            {
                CategoryList = null;
                Console.WriteLine(e.Message);
            }
            return CategoryList;
        }

    }
}
