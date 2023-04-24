namespace StoreFront.Common.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IStoreProductRepository
    {
        bool Delete(Guid storeId, Guid productId);
        List<StoreProduct> Get();
        StoreProduct GetSingle(Guid storeId, Guid productId);
        bool Insert(StoreProduct storeProduct);
    }
}