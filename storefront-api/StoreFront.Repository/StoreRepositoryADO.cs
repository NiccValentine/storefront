namespace StoreFront.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Common;
    using Common.Interfaces.Repositories;
    using Common.Models;
    using StoreFront.Common.Interfaces.Logging;

    public class StoreRepositoryADO : IStoreRepository
    {
        #region Constructors
        public StoreRepositoryADO(ILogService logService)
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
            this._logService.Debug("StoreRepositoryADO.Get called");

            var sql = "SELECT StoreId, StoreName, StoreDescription FROM Store ORDER BY StoreName";

            var stores = new List<Store>();

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Store store = this.GetStore(dataReader);
                            stores.Add(store);
                        }
                    }
                }
            }

            this._logService.Trace("StoreRepositoryADO.Get returned {0} result(s)", stores.Count);

            return stores;
        }


        public List<Store> GetStoresByProductId(Guid productId)
        {
            this._logService.Debug("StoreRepositoryADO.GetStoresByProductId called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("StoreRepositoryADO.GetStoresByProductId productId is not present");

                throw new ArgumentException(nameof(productId));
            }

            var sql = "SELECT s.StoreId, s.StoreName, s.StoreDescription FROM Store s INNER JOIN StoreProduct sp ON sp.StoreId = s.storeId WHERE sp.ProductId = @ProductId";
            
            var stores = new List<Store>();

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("ProductId", productId);

                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Store store = this.GetStore(dataReader);
                            stores.Add(store);
                        }
                    }
                }
            }

            this._logService.Trace("StoreRepositoryADO.GetStoresByProductId returned {0} result(s)", stores.Count);

            return stores;
        }

        public Store GetSingle(Guid storeId)
        {
            this._logService.Debug("StoreRepositoryADO.GetSingle called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreRepositoryADO.GetSingle storeId is not present");

                throw new ArgumentException(nameof(storeId));
            }

            var sql = "SELECT StoreId, StoreName, StoreDescription FROM Store WHERE StoreId = @StoreId";

            Store store = null;

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storeId);

                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            store = this.GetStore(dataReader);
                        }
                    }
                }
            }

            this._logService.Trace("StoreRepositoryADO.GetSingle returned 1 result");

            return store;
        }

        public bool Insert(Store store)
        {
            this._logService.Debug("StoreRepositoryADO.Insert called");

            if (store == null)
            {
                this._logService.Warn("StoreRepositoryADO.Insert store is null");

                throw new ArgumentNullException(nameof(store));
            }

            var sql = "INSERT INTO Store(StoreId, StoreName, StoreDescription) VALUES(@StoreId, @StoreName, @StoreDescription)";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    this.GetParameters(store, sqlCommand);

                    sqlConnection.Open();

                    var rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        this._logService.Trace("StoreRepositoryADO.Insert has successfully inserted data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("StoreRepositoryADO.Insert has not inserted data");

                        return false;
                    }
                }
            }
        }

        public bool Update(Store store)
        {
            this._logService.Debug("StoreRepositoryADO.Update called");

            if (store == null)
            {
                this._logService.Warn("StoreRepositoryADO.Update store is null");

                throw new ArgumentNullException(nameof(store));
            }

            var sql = "UPDATE Store SET StoreName = @StoreName, StoreDescription = @StoreDescription WHERE StoreId = @StoreId";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    this.GetParameters(store, sqlCommand);

                    sqlConnection.Open();

                    var rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        this._logService.Trace("StoreRepositoryADO.Update has successfully altered data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("StoreRepositoryADO.Update has not altered data");

                        return false;
                    }
                }
            }
        }

        public bool Delete(Guid storeId)
        {
            this._logService.Debug("StoreRepositoryADO.Delete called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreRepositoryADO.Delete storeId is null");

                throw new ArgumentException(nameof(storeId));
            }

            var sql = "DELETE Store WHERE StoreId = @StoreId";

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storeId);

                    sqlConnection.Open();

                    var rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        this._logService.Trace("StoreRepositoryADO.Delete has successfully removed data");

                        return true;
                    }
                    else
                    {
                        this._logService.Trace("StoreRepositoryADO.Delete has not removed data");

                        return false;
                    }
                }
            }
        }

        public List<Store> StoreSearch(string storeName)
        {
            this._logService.Debug("StoreRepositoryADO.StoreSearch called");

            if (storeName == null)
            {
                this._logService.Warn("StoreRepositoryADO.StoreSearch storeName is null");

                throw new ArgumentNullException(nameof(storeName));
            }

            var sql = "SELECT StoreId, StoreName, StoreDescription FROM Store WHERE LOWER(StoreName) LIKE @StoreName";

            var stores = new List<Store>();

            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                using (var sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    storeName = $"%{storeName.ToLower()}%";
                    sqlCommand.Parameters.AddWithValue("@StoreName", storeName);

                    sqlConnection.Open();

                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Store store = this.GetStore(dataReader);
                            stores.Add(store);
                        }
                    }
                }
            }

            this._logService.Trace("StoreRepositoryADO.StoreSearch returned {0} result(s)", stores.Count);

            return stores;
        }

        #endregion

        #region Private Methods

        private Store GetStore(SqlDataReader dataReader)
        {
            var store = new Store()
            {
                StoreId = (Guid)dataReader["StoreId"],
                StoreName = (string)dataReader["StoreName"],
                StoreDescription = (string)dataReader["StoreDescription"]
            };

            return store;

        }

        private void GetParameters(Store store, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("@StoreId", store.StoreId);
            sqlCommand.Parameters.AddWithValue("@StoreName", store.StoreName);
            sqlCommand.Parameters.AddWithValue("@StoreDescription", store.StoreDescription);
        }

        #endregion
    }
}
