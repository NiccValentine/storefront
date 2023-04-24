namespace StoreFront.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Common;
    using Common.Interfaces.Repositories;
    using Common.Models;

    public class StoreRepository : IStoreRepository
    {
        #region Public Methods

        public List<Store> Get()
        {
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
            return stores;
        }

        public List<Store> StoreSearch(string storeName)
        {
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
            return stores;
        }

        public List<Store> GetStoresByProductId(Guid productId)
        {
            var sql = "SELECT s.StoreId, s.StoreName, s.StoreDescription, sp.Stock, sp.Price FROM Store s INNER JOIN StoreProduct sp ON sp.StoreId = s.storeId WHERE sp.ProductId = @ProductId";
            
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
            return stores;
        }

        public Store GetSingle(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
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
            return store;
        }

        public bool Insert(Store store)
        {
            if (store == null)
            {
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool Update(Store store)
        {
            if (store == null)
            {
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool Delete(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
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
