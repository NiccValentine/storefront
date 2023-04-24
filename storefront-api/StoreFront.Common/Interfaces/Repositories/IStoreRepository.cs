namespace StoreFront.Common.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IStoreRepository
    {
        bool Delete(Guid storeId);
        List<Store> Get();
        List<Store> StoreSearch(string storeName);
        List<Store> GetStoresByProductId(Guid productId);
        Store GetSingle(Guid storeId);
        bool Insert(Store store);
        bool Update(Store store);
    }
}