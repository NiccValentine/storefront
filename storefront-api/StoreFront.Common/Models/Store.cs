namespace StoreFront.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class Store
    {
        public Guid StoreId { get; set; }

        public string StoreName { get; set; }

        public string StoreDescription { get; set; }
    }
}
