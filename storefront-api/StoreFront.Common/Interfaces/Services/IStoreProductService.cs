namespace StoreFront.Common.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IStoreProductService
    {
        ServiceResult<StoreProduct> Insert(StoreProduct storeProduct);
        ServiceResult<StoreProduct> Delete(Guid storeId, Guid productId);
    }
}