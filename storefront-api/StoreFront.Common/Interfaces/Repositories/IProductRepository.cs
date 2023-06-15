namespace StoreFront.Common.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IProductRepository
    {
        List<Product> Get();
        List<Product> GetProductsNotMatchingStoreId(Guid storeId);
        List<Product> GetProductsByStoreId(Guid storeId);
        Product GetSingle(Guid productId);
        bool Insert(Product product);
        bool Update(Product product);
        bool Delete(Guid productId);
        List<Product> ProductSearch(string productName);

    }
}