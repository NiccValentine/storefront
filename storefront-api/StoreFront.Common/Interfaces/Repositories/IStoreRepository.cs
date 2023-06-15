namespace StoreFront.Common.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IStoreRepository
    {
        List<Store> Get();
        List<Store> GetStoresByProductId(Guid productId);
        Store GetSingle(Guid storeId);
        bool Insert(Store store);
        bool Update(Store store);
        bool Delete(Guid storeId);
        List<Store> StoreSearch(string storeName);

    }
}