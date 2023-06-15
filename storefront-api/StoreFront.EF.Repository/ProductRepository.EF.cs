using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.EF.Repository.Data;

namespace StoreFront.EF.Repository
{
    public class ProductRepositoryEF : IProductRepository
    {
        public List<Product> Get()
        {
            using (StoreFrontContext context = new StoreFrontContext())
            {
                var products = context.Product.ToList();

                return products;
            }
        }
        public List<Product> GetProductsNotMatchingStoreId(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var productsInStore = context.StoreProduct
                    .Where(storeProduct => storeProduct.StoreId == storeId)
                    .Select(storeProduct => storeProduct.ProductId).ToList();
                var products = context.Product
                    .Where(product => !productsInStore.Contains(product.ProductId)).ToList();

                return products;
            }
        }
        public List<Product> GetProductsByStoreId(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
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

                return products;
            }            
        }
        public Product GetSingle(Guid productId)
        {
            if (productId == Guid.Empty) 
            {
                throw new ArgumentException(nameof(productId));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var product = context.Product
                    .Where(p => p.ProductId == productId)
                    .SingleOrDefault();

                return product;
            }
        }
        public bool Insert(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                context.Product.Add(product);

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
        public bool Update(Product product)
        {
            if (product == null)
            {
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Delete(Guid productId)
        {
            if (productId == Guid.Empty)
            {
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public List<Product> ProductSearch(string productName)
        {
            if (productName == null)
            {
                throw new ArgumentNullException(nameof(productName));
            }

            using (StoreFrontContext context = new StoreFrontContext())
            {
                var products = context.Product
                    .Where(product => product.ProductName.Contains(productName))
                    .ToList();

                return products;
            }
        }
    }
}
