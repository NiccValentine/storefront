namespace StoreFront.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class Product
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
    }
}
