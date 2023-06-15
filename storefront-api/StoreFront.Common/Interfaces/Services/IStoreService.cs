namespace StoreFront.Common.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IStoreService
    {

        List<Store> Get();
        List<Store> GetStoresByProductId(Guid productId);
        Store GetSingle(Guid storeId);
        ServiceResult<Store> Insert(Store store);
        ServiceResult<Store> Update(Store store);
        ServiceResult<Store> Delete(Guid storeId);
        List<Store> StoreSearch(string storeName);

    }
}