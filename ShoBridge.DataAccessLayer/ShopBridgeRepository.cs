using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShoBridge.DataAccessLayer.Models;

namespace ShoBridge.DataAccessLayer
{
    public class ShopBridgeRepository
    {
        ShopBridgeDBContext context = null;

        public ShopBridgeRepository()
        {
            context = new ShopBridgeDBContext();
        }

        #region AddProductImages
        public async Task<int> AddProductImageUrl(string productId, List<string> imageUrls)
        {
            int status = 0;
            try
            {
                ProductImages pi;
                foreach (var imageUrl in imageUrls)
                {
                    pi = new ProductImages();
                    pi.ImageLink = imageUrl;
                    pi.ProductId = productId;

                     context.ProductImages.Add(pi);
                }

                await context.SaveChangesAsync();
                status = 1;

            }
            catch (Exception e)
            {
                status = -99;
                Console.WriteLine(e.Message);
            }
            return status;
        } 
        #endregion

        #region AddProducts
        public async Task<string> AddProduct(Products product)
        {

            //this will either consist of the product Id that has been generated on successful addition occurs or the error code. 
            string status;
            
            int result = 0;
            try
            {

                //Adding the request product to the DataBase.
                //Calling the stored procedure to perform addition of project into the tables
                 //Generating the unique Product Id by calling the function 
                    var query = context.Products
                        .Select(d => ShopBridgeDBContext.GenerateNewProductId());


                    product.ProductId = query.First().ToString();

                    SqlParameter prmProductId = new SqlParameter("@ProductId", product.ProductId);
                    SqlParameter prmProductName = new SqlParameter("@ProductName", product.ProductName);
                    SqlParameter prmCategoryId = new SqlParameter("@CategoryId", product.CategoryId);
                    prmCategoryId.SqlDbType = System.Data.SqlDbType.Int;
                    SqlParameter prmPrice = new SqlParameter("@Price", product.Price);
                    prmPrice.SqlDbType = System.Data.SqlDbType.Float;
                    SqlParameter prmQuantityAvailable = new SqlParameter("@QuantityAvailable", product.QuantityAvailable);

                    prmQuantityAvailable.SqlDbType = System.Data.SqlDbType.Int;

                    SqlParameter returnValue = new SqlParameter("@return", System.Data.SqlDbType.Int);

                    returnValue.Direction = System.Data.ParameterDirection.Output;

                    //Calling stored procedure
                    await context.Database.ExecuteSqlRawAsync("EXEC @return = usp_AddProduct @ProductId, @ProductName, @CategoryId, @Price, @QuantityAvailable", new[] { returnValue, prmProductId, prmProductName, prmCategoryId, prmPrice, prmQuantityAvailable });
                    result = Convert.ToInt32(returnValue.Value);

                    //on successful attempt to add the product, adding the PID else will add the error code in the response. 
                    if (result == 1)
                    {
                        status = product.ProductId;
                    }
                    else
                    {
                        status = result.ToString();
                    }
                
            }
            catch (Exception e)
            {
                result = -99;
                status = "Exception caught at DAL";
                Console.WriteLine(e.Message);
                return status;
            }
            return status;
        }
        #endregion

        #region ModifyProduct
        public async Task<string> ModifyProduct(Products product)
        {
            int result = 0;
            string status;
            try
            {
                SqlParameter prmProductId = new SqlParameter("@ProductId", product.ProductId);
                SqlParameter prmProductName = new SqlParameter("@ProductName", product.ProductName);
                SqlParameter prmCategoryId = new SqlParameter("@CategoryId", product.CategoryId);
                prmCategoryId.SqlDbType = System.Data.SqlDbType.Int;
                SqlParameter prmPrice = new SqlParameter("@Price", product.Price);
                prmPrice.SqlDbType = System.Data.SqlDbType.Float;
                SqlParameter prmQuantityAvailable = new SqlParameter("@QuantityAvailable", product.QuantityAvailable);

                prmQuantityAvailable.SqlDbType = System.Data.SqlDbType.Int;

                SqlParameter returnValue = new SqlParameter("@return", System.Data.SqlDbType.Int);

                returnValue.Direction = System.Data.ParameterDirection.Output;

                await context.Database.ExecuteSqlRawAsync("EXEC @return = usp_ModifyProduct @ProductId, @ProductName, @CategoryId, @Price, @QuantityAvailable", new[] { returnValue, prmProductId, prmProductName, prmCategoryId, prmPrice, prmQuantityAvailable });
                result = Convert.ToInt32(returnValue.Value);
                status = result.ToString();
            }
            catch (Exception e)
            {
                result = -99;
                status = "Exception caught at DAL";
                Console.WriteLine(e.Message);
                return status;
            }
            return status; 
        }
        #endregion

        #region DeleteProduct
        public async Task<int> DeleteProduct(string productId)
        {
            int result = 0;
            try
            {
                var product = from products in context.Products
                                    where products.ProductId == productId
                                    select products;


                foreach (var detail in product)
                {
                    context.Products.Remove(detail);
                }
               

                await context.SaveChangesAsync();
                result = 1;
            }
            catch (Exception e)
            {
                result = -99;
                Console.WriteLine(e.Message);
            }
            return result;
        }
        #endregion

        #region GetAllProductsInInventory
        public List<Products> GetAllProducts()
        {
            List<Products> ProductList = null;
            try
            {
                ProductList = context.Products.FromSqlRaw("SELECT * FROM dbo.Products").ToList();
            }
            catch (Exception e)
            {
                ProductList = null;
                Console.WriteLine(e.Message);
            }
            return ProductList;
        }
        #endregion

        #region GetAllCategoriesInInventory
        public List<Categories> GetAllCategories()
        {
            List<Categories> CategoryList = null;
            try
            {
                CategoryList = context.Categories.FromSqlRaw("SELECT * FROM dbo.ufn_GetCategories()").ToList();
            }
            catch (Exception e)
            {
                CategoryList = null;
                Console.WriteLine(e.Message);
            }
            return CategoryList;
        }
        #endregion

    }
}
