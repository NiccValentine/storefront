using StoreFront.Common.Interfaces.Logging;
using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.EF.Repository.Data;

namespace StoreFront.EF.Repository
{
    public class StoreProductRepositoryEF : IStoreProductRepository
    {
        #region Constructors
        public StoreProductRepositoryEF(ILogService logService)
        {
            this._logService = logService;
        }
        #endregion

        #region Private Properties
        private ILogService _logService;
        #endregion

        #region Public Methods
        public bool Insert(StoreProduct storeProduct)
        {
            this._logService.Debug("StoreProductRepositoryEF.Insert called");

            if (storeProduct == null)
            {
                this._logService.Warn("StoreProductRepositoryEF.Insert storeProduct is null");

                throw new ArgumentNullException(nameof(storeProduct));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                context.StoreProduct.Add(storeProduct);

                var rowsAffected = context.SaveChanges();

                if (rowsAffected == 1)
                {
                    this._logService.Trace("StoreProductRepositoryEF.Insert has successfully inserted data");

                    return true;
                }
                else
                {
                    this._logService.Trace("StoreProductRepositoryEF.Insert has not inserted data");

                    return false;
                }
            }
        }
        public bool Delete(Guid storeId, Guid productId)
        {
            this._logService.Debug("StoreProductRepositoryEF.Delete called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreProductRepositoryEF.Delete storeId is null");

                throw new ArgumentException(nameof(storeId));
            }

             if (productId == Guid.Empty)
            {
                this._logService.Warn("StoreProductRepositoryEF.Delete productId is null");

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
                    this._logService.Trace("StoreProductRepositoryEF.Delete has successfully removed data");

                    return true;
                }
                else
                {
                    this._logService.Trace("StoreProductRepositoryEF.Delete has not removed data");

                    return false;
                }
            }
        }
        #endregion
    }
}

