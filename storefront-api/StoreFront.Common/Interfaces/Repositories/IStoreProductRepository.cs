namespace StoreFront.Common.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IStoreProductRepository
    {
        bool Insert(StoreProduct storeProduct);
        bool Delete(Guid storeId, Guid productId);
    }
}