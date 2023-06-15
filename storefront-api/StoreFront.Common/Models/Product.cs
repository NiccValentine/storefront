namespace StoreFront.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class Product
    {
        #region ADO
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        #endregion

        #region Entity Framework
        public List<StoreProduct>? StoreProduct { get; set; }
        #endregion
    }
}
