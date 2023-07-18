namespace StoreFront.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Common;
    using Common.Interfaces.Repositories;
    using Common.Models;
    using StoreFront.Common.Interfaces.Logging;

    public class ProductRepositoryADO : IProductRepository
    {

        #region Constructors
        public ProductRepositoryADO(ILogService logService)
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
            this._logService.Debug("ProductRepositoryADO.Get called");

            var sql = "SELECT ProductId, ProductName, ProductDescription FROM Product ORDER BY ProductName";

            var products = new List<Product>();

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Product product = this.GetProduct(dataReader);
                            products.Add(product);
                        }
                    }
                }
            }

            this._logService.Trace("ProductRepositoryADO.Get returned {0} result(s)", products.Count);

            return products;
        }

        public List<Product> GetProductsNotMatchingStoreId(Guid storeId)
        {
            this._logService.Debug("ProductRepositoryADO.GetProductsNotMatchingStoreId called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryADO.Get storeId is not present");

                throw new ArgumentException(nameof(storeId));
            }

            var sql = "SELECT * FROM Product P1 WHERE P1.ProductId NOT IN(SELECT P2.ProductId FROM Product P2 INNER JOIN StoreProduct SP ON SP.ProductId = P2.ProductId WHERE SP.StoreId = @StoreId)";

            var products = new List<Product>();

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storeId);

                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Product product = this.GetProduct(dataReader);
                            products.Add(product);
                        }
                    }
                }
            }

            this._logService.Trace("ProductRepositoryADO.GetProductsNotMatchingStoreId returned {0} result(s)", products.Count);

            return products;
        }


        public List<Product> GetProductsByStoreId(Guid storeId)
        {
            this._logService.Debug("ProductRepositoryADO.GetProductsByStoreId called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryADO.GetProductsByStoreId storeId is not present");

                throw new ArgumentException(nameof(storeId));
            }

            var sql = "SELECT p.ProductId, p.ProductName, p.ProductDescription FROM Product p INNER JOIN StoreProduct sp ON sp.ProductId = p.ProductId WHERE sp.StoreId = @StoreId";

            var products = new List<Product>();

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storeId);

                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Product product = this.GetProduct(dataReader);
                            products.Add(product);
                        }
                    }
                }
            }

            this._logService.Trace("ProductRepositoryADO.GetProductsByStoreId returned {0} result(s)", products.Count);

            return products;
        }

        public Product GetSingle(Guid productId)
        {
            this._logService.Debug("ProductRepositoryADO.GetSingle called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryADO.GetSingle productId is not present");

                throw new ArgumentException(nameof(productId));
            }

            var sql = "SELECT ProductId, ProductName, ProductDescription FROM Product WHERE ProductId = @ProductId";

            Product product = null;

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ProductId", productId);

                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            product = this.GetProduct(dataReader);
                        }
                    }
                }
            }

            this._logService.Trace("ProductRepositoryADO.GetSingle returned 1 result");

            return product;
        }

        public bool Insert(Product product)
        {
            this._logService.Debug("ProductRepositoryADO.Insert called");

            if (product == null)
            {
                this._logService.Warn("ProductRepositoryADO.Insert product is null");

                throw new ArgumentNullException(nameof(product));
            }

            var sql = "INSERT INTO Product(ProductId, ProductName, ProductDescription) VALUES (@ProductId, @ProductName, @ProductDescription)";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    this.GetParameters(product, sqlCommand);

                    sqlConnection.Open();

                    var rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        this._logService.Trace("ProductRepositoryADO.Insert has successfully inserted data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("ProductRepositoryADO.Insert has not inserted data");

                        return false;
                    }
                }
            }
        }

        public bool Update(Product product)
        {
            this._logService.Debug("ProductRepositoryADO.Update called");

            if (product == null)
            {
                this._logService.Warn("ProductRepositoryADO.Update product is null");

                throw new ArgumentNullException(nameof(product));
            }

            var sql = "UPDATE Product SET ProductName = @ProductName, ProductDescription = @ProductDescription WHERE ProductId = @ProductId";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    this.GetParameters(product, sqlCommand);

                    sqlConnection.Open();

                    var rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        this._logService.Trace("ProductRepositoryADO.Update has successfully altered data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("ProductRepositoryADO.Update has not altered data");

                        return false;
                    }
                }
            }
        }

        public bool Delete(Guid productId)
        {
            this._logService.Debug("ProductRepositoryADO.Delete called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("ProductRepositoryADO.Delete productId is null");

                throw new ArgumentException(nameof(productId));
            }

            var sql = "DELETE Product WHERE ProductId = @ProductId";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ProductId", productId);

                    sqlConnection.Open();

                    var rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        this._logService.Trace("ProductRepositoryADO.Delete has successfully removed data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("ProductRepositoryADO.Delete has not removed data");

                        return false;
                    }
                }
            }
        }

        public List<Product> ProductSearch(string productName)
        {
            this._logService.Debug("ProductRepositoryADO.ProductSearch called");

            if (productName == null)
            {
                this._logService.Warn("ProductRepositoryADO.ProductSearch productId is null");

                throw new ArgumentNullException(nameof(productName));
            }

            var sql = "SELECT ProductId, ProductName, ProductDescription FROM Product WHERE LOWER(ProductName) LIKE @ProductName";

            var products = new List<Product>();

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    productName = $"%{productName.ToLower()}%";
                    sqlCommand.Parameters.AddWithValue("@ProductName", productName);

                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Product product = this.GetProduct(dataReader);
                            products.Add(product);
                        }
                    }
                }
            }

            this._logService.Trace("ProductRepositoryEF.ProductSearch returned {0} result(s)", products.Count);

            return products;
        }
        #endregion

        #region Private Methods

        private Product GetProduct(SqlDataReader dataReader)
        {
            return new Product()
            {
                ProductId = (Guid)dataReader["ProductId"],
                ProductName = (string)dataReader["ProductName"],
                ProductDescription = (string)dataReader["ProductDescription"]
            };
        }

        private void GetParameters(Product product, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
            sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
            sqlCommand.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
        }
        #endregion

    }
}
