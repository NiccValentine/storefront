namespace StoreFront.Repository
{
    using System;
    using System.Data.SqlClient;
    using Common;
    using Common.Interfaces.Repositories;
    using Common.Models;
    using StoreFront.Common.Interfaces.Logging;

    public class StoreProductRepositoryADO : IStoreProductRepository
    {
        #region Constructors
        public StoreProductRepositoryADO(ILogService logService)
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
            this._logService.Warn("StoreProductRepositoryADO.Insert storeProduct is null");

            if (storeProduct == null)
            {
                this._logService.Warn("StoreProductRepositoryADO.Insert storeProduct is null");

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
                        this._logService.Trace("StoreProductRepositoryADO.Insert has successfully inserted data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("StoreProductRepositoryADO.Insert has not inserted data");

                        return false;
                    }
                }
            }
        }

        public bool Delete(Guid storeId, Guid productId)
        {
            this._logService.Debug("StoreProductRepositoryADO.Delete called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreProductRepositoryADO.Delete storeId is null");

                throw new ArgumentException(nameof(storeId));
            }

            if (productId == Guid.Empty)
            {
                this._logService.Warn("StoreProductRepositoryADO.Delete productId is null");

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
                        this._logService.Trace("StoreProductRepositoryADO.Delete has successfully removed data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("StoreProductRepositoryADO.Delete has not removed data");

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
