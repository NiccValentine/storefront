using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.EF.Repository.Data;

namespace StoreFront.EF.Repository
{
    public class StoreProductRepositoryEF : IStoreProductRepository
    {
        public bool Insert(StoreProduct storeProduct)
        {
            if (storeProduct == null)
            {
                throw new ArgumentNullException(nameof(storeProduct));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                context.StoreProduct.Add(storeProduct);

                var rowsAffected = context.SaveChanges();

                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Delete(Guid storeId, Guid productId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

             if (productId == Guid.Empty)
            {
                throw new ArgumentException(nameof(productId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var storeProductDelete = context.StoreProduct
                    .Where(
                    storeProduct => storeProduct.StoreId == storeId 
                    && storeProduct.ProductId == productId)
                    .SingleOrDefault();

                if (storeProductDelete != null)
                {
                    context.StoreProduct.Remove(storeProductDelete);
                }

                var rowsAffected = context.SaveChanges();

                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

