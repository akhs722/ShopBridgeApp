using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.ServiceLayer.ViewModels
{
    public class Product
    {     
            public string Status { get; set; }
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public byte? CategoryId { get; set; }
            public decimal Price { get; set; }
            public int QuantityAvailable { get; set; }
            public List<string> ImageUrls { get; set; }
    }
}
