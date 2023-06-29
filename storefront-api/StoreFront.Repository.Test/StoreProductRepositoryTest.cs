using System;
using StoreFront.Common.Models;
using SqlTestPrep;
using Xunit;

namespace StoreFront.Repository.Test
{
    [Collection("Sequential")]
    public class StoreProductRepositoryTest : IDisposable
    {
        #region Public Constructors
        public StoreProductRepositoryTest()
        {
            _sqlLoader = new SqlLoader();

            _storeProductRepository = new StoreProductRepository();

            try
            {
                _sqlLoader.Setup("storefronttest");
            }
            catch (Exception e)
            {
                _sqlLoader.TearDown("storefronttest");
            }
        }
        #endregion

        #region Private Constructors

        private SqlLoader _sqlLoader { get; }

        private StoreProductRepository _storeProductRepository { get; }

        #endregion

        #region Tests

        [Fact]
        public void Insert_Success()
        {
            var storeProduct = new StoreProduct()
            {
                StoreId = Guid.Parse("7d43aab0-4a94-4556-b2ae-b131e86f4c25"),
                ProductId = Guid.Parse("f0743f41-4c22-432a-9501-1855bdc5f887"),
            };

            var result = this._storeProductRepository.Insert(storeProduct);

            Assert.True(result);
        }

        [Fact]
        public void Insert_Exception()
        {
            StoreProduct storeProduct = null;

            Assert.Throws<ArgumentNullException>(() => this._storeProductRepository.Insert(storeProduct));
        }

        [Fact]
        public void Delete_Success()
        {
            var result = this._storeProductRepository.Delete(Guid.Parse("c25ef7e4-5641-40ac-b945-306fd3efc04b"), Guid.Parse("f1c2cf3e-b65f-4991-84a9-eca8dca3a08a"));

            Assert.True(result);
        }

        [Fact]
        public void Delete_Failure1()
        {
            var result = this._storeProductRepository.Delete(Guid.Parse("83fdc303-2b14-4436-a1dc-5a9dad9f959a"), Guid.Parse("f1c2cf3e-b65f-4991-84a9-eca8dca3a08a"));

            Assert.False(result);
        }

        [Fact]
        public void Delete_Failure2()
        {
            var result = this._storeProductRepository.Delete(Guid.Parse("c25ef7e4-5641-40ac-b945-306fd3efc04b"), Guid.Parse("f6bde077-e480-4cbb-aec9-8529619a6b0a"));

            Assert.False(result);
        }

        [Fact]
        public void Delete_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._storeProductRepository.Delete(Guid.Empty, Guid.Empty));
        }

        #endregion

        #region Disposal

        public void Dispose()
        {
            _sqlLoader.TearDown("storefronttest");
        }

        #endregion
    }
}
