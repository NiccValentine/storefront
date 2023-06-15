namespace StoreFront.Common.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IProductService
    {
        List<Product> Get();
        List<Product> GetProductsNotMatchingStoreId(Guid storeId);
        List<Product> GetProductsByStoreId(Guid storeId);
        Product GetSingle(Guid productId);
        ServiceResult<Product> Insert(Product product);
        ServiceResult<Product> Update(Product product);
        ServiceResult<Product> Delete(Guid productId);
        List<Product> ProductSearch(string productName);

    }
}