using Microsoft.EntityFrameworkCore;
using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.EF.Repository.Data;

namespace StoreFront.EF.Repository
{
    public class StoreRepositoryEF : IStoreRepository
    {
        public List<Store> Get()
        {
            using (StoreFrontContext context = new StoreFrontContext())
            {
                var stores = context.Store.ToList();

                return stores;
            }
        }
        public List<Store> GetStoresByProductId(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException(nameof(productId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var stores = context.StoreProduct
                    .Where(storeProduct => storeProduct.ProductId == productId)
                    .Join(
                    context.Store,
                    storeProduct => storeProduct.StoreId,
                    store => store.StoreId,
                    (storeProduct, store) => store)
                    .ToList();

                return stores;
            }
        }
        public Store GetSingle(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var store = context.Store
                    .Where(s => s.StoreId == storeId)
                    .SingleOrDefault();

                return store;
            }
        }
        public bool Insert(Store store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                context.Store.Add(store);

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
        public bool Update(Store store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var storeUpdate = context.Store
                    .Where(storeId => storeId.StoreId == store.StoreId)
                    .SingleOrDefault();

                storeUpdate.StoreId = store.StoreId;
                storeUpdate.StoreName = store.StoreName;
                storeUpdate.StoreDescription = store.StoreDescription;

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
        public bool Delete(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var storeDelete = context.Store
                    .Where(store => store.StoreId == storeId)
                    .SingleOrDefault();

                if (storeDelete != null)
                {
                    context.Store.Remove(storeDelete);
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
        public List<Store> StoreSearch(string storeName)
        {
            if (storeName == null)
            {
                throw new ArgumentNullException(nameof(storeName));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var stores = context.Store
                    .Where(store => store.StoreName.Contains(storeName))
                    .ToList();

                return stores;
            }
        }
    }
}
