using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoBridge.DataAccessLayer;
using ShoBridge.DataAccessLayer.Models;

namespace Testing_DataAccessLayer
{
    class Program
    {
        static async Task Main(string[] args)
        {
           

            TestingDal td = new TestingDal();

            string result1 = await td.Testing_AddProduct();
            Console.WriteLine(result1);
            string result2 =  await td.Testing_ModifyProduct();
            Console.WriteLine(result2);
            int result3 =  await td.Testing_DeleteProduct();
            Console.WriteLine(result3);
            var result4 = td.Testing_GetAllProducts();
            for (int i = 0; i < result4.Count; i++)
            {
                Console.WriteLine($"Product Name: {result4[i].ProductName}");
                Console.WriteLine($"Product Id: {result4[i].ProductId}");
                Console.WriteLine($"Category Id: {result4[i].CategoryId}");
                Console.WriteLine($"Price: {result4[i].Price}");
                Console.WriteLine($"Quantity Available: {result4[i].QuantityAvailable}");
            }
            
            var result5 = td.Testing_GetAllCategories();

            for (int i = 0; i < result5.Count; i++)
            {
                Console.WriteLine($"Product Name: {result5[i].CategoryId}");
                Console.WriteLine($"Product Id: {result5[i].CategoryName}");
               
            }


        }

       
    }
    public class TestingDal
    {
        ShopBridgeRepository sb;
        public TestingDal()
        {
            sb = new ShopBridgeRepository();
        }
        public async Task<string> Testing_AddProduct()
        {
            ShoBridge.DataAccessLayer.Models.Products p = new ShoBridge.DataAccessLayer.Models.Products();
            p.Price = 128;
            p.ProductName = "Apple Iphone";
            p.QuantityAvailable = 15;
            p.CategoryId = 5;

            string result = await sb.AddProduct(p);
            
            return result;

        }

        public async Task<string> Testing_ModifyProduct()
        {
            ShoBridge.DataAccessLayer.Models.Products p = new ShoBridge.DataAccessLayer.Models.Products();
            p.Price = 128;
            p.ProductName = "Apple Iphone";
            p.QuantityAvailable = 15;
            p.CategoryId = 5;
            p.ProductId = "P156";
            string result = await sb.ModifyProduct(p);

            return result;

        }

        public async Task<int> Testing_DeleteProduct()
        {

            string productId = "P157"; 
            int result = await sb.DeleteProduct(productId);

            return result;

        }
        public List<Products> Testing_GetAllProducts()
        {

            var result =  sb.GetAllProducts();
            return result;

        }
        public List<Categories> Testing_GetAllCategories()
        {

            var result = sb.GetAllCategories();
            return result;

        }


    }
}
