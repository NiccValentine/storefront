using StoreFront.Common.Interfaces.Logging;
using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.EF.Repository.Data;

namespace StoreFront.EF.Repository
{

    public class ProductRepositoryEF : IProductRepository
    {
        #region Constructors
        public ProductRepositoryEF(ILogService logService)
        {
            this._logService = logService;
        }
        #endregion

        #region Private Properties
        private ILogService _logService;
        #endregion

        #region Public Methods

        public List<Product> Get()
        {
            this._logService.Debug("ProductRepositoryEF.Get called");

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var products = context.Product.ToList();

                this._logService.Trace("ProductRepositoryEF.Get returned {0} result(s)", products.Count);

                return products;
            }
        }
        public List<Product> GetProductsNotMatchingStoreId(Guid storeId)
        {
            this._logService.Debug("ProductRepositoryEF.GetProductsMatchingStoreId called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryEF.GetProductsNotMatchingStoreId storeId is not present");

                throw new ArgumentException(nameof(storeId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var productsInStore = context.StoreProduct
                    .Where(storeProduct => storeProduct.StoreId == storeId)
                    .Select(storeProduct => storeProduct.ProductId).ToList();
                var products = context.Product
                    .Where(product => !productsInStore.Contains(product.ProductId)).ToList();

                this._logService.Trace("ProductRepositoryEF.GetProductsNotMatchingStoreId returned {0} result(s)", products.Count);

                return products;
            }
        }
        public List<Product> GetProductsByStoreId(Guid storeId)
        {
            this._logService.Debug("ProductRepositoryEF.GetProductsByStoreId called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryEF.GetProductsByStoreId storeId is not present");

                throw new ArgumentException(nameof(storeId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var products = context.StoreProduct
                    .Where(storeProduct => storeProduct.StoreId == storeId)
                    .Join(
                    context.Product,
                    storeProduct => storeProduct.ProductId,
                    product => product.ProductId,
                    (storeProduct, product) => product)
                    .ToList();

                this._logService.Trace("ProductRepositoryEF.GetProductsByStoreId returned {0} result(s)", products.Count);

                return products;
            }            
        }
        public Product GetSingle(Guid productId)
        {
            this._logService.Debug("ProductRepositoryEF.GetSingle called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryEF.GetSingle productId is not present");

                throw new ArgumentException(nameof(productId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var product = context.Product
                    .Where(p => p.ProductId == productId)
                    .SingleOrDefault();

                this._logService.Trace("ProductRepositoryEF.GetSingle returned 1 result");

                return product;
            }
        }
        public bool Insert(Product product)
        {
            this._logService.Debug("ProductRepositoryEF.Insert called");

            if (product == null)
            {
                this._logService.Warn("ProductRepositoryEF.Insert product is null");

                throw new ArgumentNullException(nameof(product));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                context.Product.Add(product);

                var rowsAffected = context.SaveChanges();

                if (rowsAffected == 1)
                {
                    this._logService.Trace("ProductRepositoryEF.Insert has successfully inserted data");

                    return true;
                }
                else
                {
                    this._logService.Trace("ProductRepositoryEF.Insert has not inserted data");

                    return false;
                }
            }
        }
        public bool Update(Product product)
        {
            this._logService.Debug("ProductRepositoryEF.Update called");

            if (product == null)
            {
                this._logService.Warn("ProductRepositoryEF.Update product is null");

                throw new ArgumentNullException(nameof(product));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var productUpdate = context.Product
                    .Where(productId => productId.ProductId == product.ProductId)
                    .SingleOrDefault();

                productUpdate.ProductId = product.ProductId;
                productUpdate.ProductName = product.ProductName;
                productUpdate.ProductDescription = product.ProductDescription;

                var rowsAffected = context.SaveChanges();

                if (rowsAffected == 1)
                {
                    this._logService.Trace("ProductRepositoryEF.Update has successfully altered data");

                    return true;
                }
                else
                {
                    this._logService.Trace("ProductRepositoryEF.Update has not altered data");

                    return false;
                }
            }
        }
        public bool Delete(Guid productId)
        {
            this._logService.Debug("ProductRepositoryEF.Delete called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryEF.Delete productId is null");

                throw new ArgumentException(nameof(productId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var productDelete = context.Product
                    .Where(product => product.ProductId == productId)
                    .SingleOrDefault();

                if (productDelete != null)
                {
                    context.Product.Remove(productDelete);
                }

                var rowsAffected = context.SaveChanges();

                if (rowsAffected == 1)
                {
                    this._logService.Trace("ProductRepositoryEF.Delete has successfully removed data");

                    return true;
                }
                else
                {
                    this._logService.Trace("ProductRepositoryEF.Delete has not removed data");

                    return false;
                }
            }
        }
        public List<Product> ProductSearch(string productName)
        {
            this._logService.Debug("ProductRepositoryEF.ProductSearch called");

            if (productName == null)
            {
                this._logService.Warn("ProductRepositoryEF.ProductSearch productId is null");

                throw new ArgumentNullException(nameof(productName));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var products = context.Product
                    .Where(product => product.ProductName.Contains(productName))
                    .ToList();

                this._logService.Trace("ProductRepositoryEF.ProductSearch returned {0} result(s)", products.Count);

                return products;
            }
        }
        #endregion
    }
}
