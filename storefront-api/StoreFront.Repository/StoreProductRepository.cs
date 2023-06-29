namespace StoreFront.Repository
{
    using System;
    using System.Data.SqlClient;
    using Common;
    using Common.Interfaces.Repositories;
    using Common.Models;

    public class StoreProductRepository : IStoreProductRepository
    {
        #region Public Methods

        public bool Insert(StoreProduct storeProduct)
        {
            if (storeProduct == null)
            {
                throw new ArgumentNullException(nameof(storeProduct));
            }

            var sql = "INSERT INTO StoreProduct(StoreId, ProductId) VALUES(@StoreId, @ProductId)";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    this.GetParameters(storeProduct, sqlCommand);

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

            var sql = "DELETE StoreProduct WHERE StoreId = @StoreId AND ProductId = @ProductId";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storeId);
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
        #endregion

        #region Private Methods

        private void GetParameters(StoreProduct storeProduct, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("StoreId", storeProduct.StoreId);
            sqlCommand.Parameters.AddWithValue("ProductId", storeProduct.ProductId);
        }
        #endregion

    }
}
