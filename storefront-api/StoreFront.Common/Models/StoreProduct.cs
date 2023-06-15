namespace StoreFront.Common.Models
{
    using System;

    public class StoreProduct
    {
        #region ADO
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        #endregion

        #region Entity Framework
        public Product? Product { get; set; }
        public Store? Store { get; set; }
     #endregion
    }
}
