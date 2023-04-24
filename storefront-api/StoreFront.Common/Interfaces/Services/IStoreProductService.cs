namespace StoreFront.Common.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IStoreProductService
    {
        ServiceResult<StoreProduct> Delete(Guid storeId, Guid productId);
        List<StoreProduct> Get();
        StoreProduct GetSingle(Guid storeId, Guid productId);
        ServiceResult<StoreProduct> Insert(StoreProduct storeProduct);
    }
}