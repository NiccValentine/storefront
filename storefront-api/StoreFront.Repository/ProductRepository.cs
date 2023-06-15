namespace StoreFront.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Common;
    using Common.Interfaces.Repositories;
    using Common.Models;

    public class ProductRepository : IProductRepository
    {

        #region Public Methods

        public List<Product> Get()
        {
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
            return products;
        }

        public List<Product> GetProductsNotMatchingStoreId(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
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
            return products;
        }


        public List<Product> GetProductsByStoreId(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
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
            return products;
        }

        public Product GetSingle(Guid productId)
        {
            if (productId == Guid.Empty)
            {
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
            return product;
        }

        public bool Insert(Product product)
        {
            if (product == null)
            {
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool Update(Product product)
        {
            if (product == null)
            {
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool Delete(Guid productId)
        {
            if (productId == Guid.Empty)
            {
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public List<Product> ProductSearch(string productName)
        {
            if (productName == null)
            {
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
