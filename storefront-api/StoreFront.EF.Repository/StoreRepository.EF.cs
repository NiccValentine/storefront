using Microsoft.EntityFrameworkCore;
using StoreFront.Common.Interfaces.Logging;
using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.EF.Repository.Data;

namespace StoreFront.EF.Repository
{
    public class StoreRepositoryEF : IStoreRepository
    {
        #region Constructors
        public StoreRepositoryEF(ILogService logService)
        {
            this._logService = logService;
        }
        #endregion

        #region Private Properties
        private ILogService _logService;
        #endregion

        #region Public Methods
        public List<Store> Get()
        {
            this._logService.Debug("StoreRepositoryEF.Get called");

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var stores = context.Store.ToList();

                this._logService.Trace("StoreRepositoryEF.Get returned {0} result(s)", stores.Count);

                return stores;
            }
        }
        public List<Store> GetStoresByProductId(Guid productId)
        {
            this._logService.Debug("StoreRepositoryEF.GetStoresByProductId called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("StoreRepositoryEF.GetStoresByProductId productId is not present");

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

                this._logService.Trace("StoreRepositoryEF.GetStoresByProductId returned {0} result(s)", stores.Count);

                return stores;
            }
        }
        public Store GetSingle(Guid storeId)
        {
            this._logService.Debug("StoreRepositoryEF.GetSingle called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreRepositoryEF.GetSingle storeId is not present");

                throw new ArgumentException(nameof(storeId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var store = context.Store
                    .Where(s => s.StoreId == storeId)
                    .SingleOrDefault();

                this._logService.Trace("StoreRepositoryEF.GetSingle returned 1 result");

                return store;
            }
        }
        public bool Insert(Store store)
        {
            this._logService.Debug("StoreRepositoryEF.Insert called");

            if (store == null)
            {
                this._logService.Warn("StoreRepositoryEF.Insert store is null");

                throw new ArgumentNullException(nameof(store));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                context.Store.Add(store);

                var rowsAffected = context.SaveChanges();

                if (rowsAffected == 1)
                {
                    this._logService.Trace("StoreRepositoryEF.Insert has successfully inserted data");

                    return true;
                }
                else
                {
                    this._logService.Trace("StoreRepositoryEF.Insert has not inserted data");

                    return false;
                }
            }
        }
        public bool Update(Store store)
        {
            this._logService.Debug("StoreRepositoryEF.Update called");

            if (store == null)
            {
                this._logService.Warn("StoreRepositoryEF.Update store is null");

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
                    this._logService.Trace("StoreRepositoryEF.Update has successfully altered data");

                    return true;
                }
                else
                {
                    this._logService.Trace("StoreRepositoryEF.Update has not altered data");

                    return false;
                }
            }
        }
        public bool Delete(Guid storeId)
        {
            this._logService.Debug("StoreRepositoryEF.Delete called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreRepositoryEF.Delete storeId is null");

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
                    this._logService.Trace("StoreRepositoryEF.Delete has successfully removed data");

                    return true;
                }
                else
                {
                    this._logService.Trace("StoreRepositoryEF.Delete has not removed data");

                    return false;
                }
            }
        }
        public List<Store> StoreSearch(string storeName)
        {
            this._logService.Debug("StoreRepositoryEF.StoreSearch called");

            if (storeName == null)
            {
                this._logService.Warn("StoreRepositoryEF.StoreSearch storeName is null");

                throw new ArgumentNullException(nameof(storeName));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var stores = context.Store
                    .Where(store => store.StoreName.Contains(storeName))
                    .ToList();

                this._logService.Trace("StoreRepositoryEF.StoreSearch returned {0} result(s)", stores.Count);

                return stores;
            }
        }
        #endregion
    }
}
