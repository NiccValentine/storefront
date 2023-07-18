using System;
using StoreFront.Common.Models;
using SqlTestPrep;
using Xunit;
using StoreFront.Common.Logging;

namespace StoreFront.Repository.Test
{
    [Collection("Sequential")]
    public class StoreRepositoryTest : IDisposable
    {
        public StoreRepositoryTest()
        {
            this._sqlLoader = new SqlLoader();

            this._storeRepository = new StoreRepositoryADO(new LogService());

            try
            {
                this._sqlLoader.Setup("storefronttest");
            }
            catch (Exception e)
            {
                this._sqlLoader.TearDown("storefronttest");
            }
        }

        private SqlLoader _sqlLoader { get; }

        private StoreRepositoryADO _storeRepository { get; }

        #region Tests
        [Fact]
        public void Get_Success()
        {
            var result = this._storeRepository.Get();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetStoresByProductId_Success()
        {
            var result = this._storeRepository.GetStoresByProductId(Guid.Parse("f1c2cf3e-b65f-4991-84a9-eca8dca3a08a"));

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void GetStoresByProductId_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._storeRepository.GetStoresByProductId(Guid.Empty));
        }

        [Fact]
        public void GetSingle_Success()
        {
            var result = this._storeRepository.GetSingle(Guid.Parse("ff0c12cc-c79a-428d-aafc-7a93970b01cd"));

            Assert.NotNull(result);
        }

        [Fact]
        public void GetSingle_Failure()
        {
            var result = this._storeRepository.GetSingle(Guid.Parse("58c2e109-d57b-4832-b477-eba01ebdc0ae"));

            Assert.Null(result);
        }

        [Fact]
        public void GetSingle_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._storeRepository.GetSingle(Guid.Empty));
        }

        [Fact]
        public void Insert_Success()
        {
            var store = new Store()
            {
                StoreId = Guid.Parse("3f79d7aa-f686-4279-befc-314e99e88086"),
                StoreName = "Barns & Noble",
                StoreDescription = "America's most recognizable bookstore"
            };

            var result = this._storeRepository.Insert(store);

            Assert.True(result);
        }

        [Fact]
        public void InsertException()
        {
            Store store = null;

            Assert.Throws<ArgumentNullException>(() => this._storeRepository.Insert(store));
        }

        [Fact]
        public void Update_Success()
        {
            var store = new Store()
            {
                StoreId = Guid.Parse("c25ef7e4-5641-40ac-b945-306fd3efc04b"),
                StoreName = "Nick's Knacks",
                StoreDescription = "A kitchy shop full of handmade and unique goods"
            };

            var result = this._storeRepository.Update(store);

            Assert.True(result);
        }

        [Fact]
        public void Update_Exception()
        {
            Store store = null;

            Assert.Throws<ArgumentNullException>(() => this._storeRepository.Update(store));
        }

        [Fact]
        public void Delete_Success()
        {
            var result = this._storeRepository.Delete(Guid.Parse("7d43aab0-4a94-4556-b2ae-b131e86f4c25"));

            Assert.True(result);
        }

        [Fact]
        public void Delete_Failure()
        {
            var result = this._storeRepository.Delete(Guid.Parse("3fb09172-80a2-45b1-a897-35d4908d6b9b"));

            Assert.False(result);
        }

        [Fact]
        public void Delete_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._storeRepository.Delete(Guid.Empty));
        }

        [Fact]
        public void StoreSearch_Success1()
        {
            var result = this._storeRepository.StoreSearch("");

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void StoreSearch_Success2()
        {
            var result = this._storeRepository.StoreSearch("store2");

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void StoreSearch_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => this._storeRepository.StoreSearch(null));
        }
        #endregion

        #region Disposal
        public void Dispose()
        {
            this._sqlLoader.TearDown("storefronttest");
        }
        #endregion
    }
}
