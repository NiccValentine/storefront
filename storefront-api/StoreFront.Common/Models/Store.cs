namespace StoreFront.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class Store
    {
        #region ADO
        public Guid StoreId { get; set; }

        public string StoreName { get; set; }

        public string StoreDescription { get; set; }
        #endregion

        #region Entity Framework
        public List<StoreProduct>? StoreProduct { get; set; }
        #endregion
    }
}
