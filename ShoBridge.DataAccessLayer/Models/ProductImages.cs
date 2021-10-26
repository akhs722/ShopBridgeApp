using System;
using System.Collections.Generic;

namespace ShoBridge.DataAccessLayer.Models
{
    public partial class ProductImages
    {
        public int Sno { get; set; }
        public string ProductId { get; set; }
        public string ImageLink { get; set; }

        public virtual Products Product { get; set; }
    }
}
