using System;
using SqlTestPrep;
using Xunit;
using StoreFront.Common.Models;
using StoreFront.EF.Repository;
using StoreFront.Common.Logging;

namespace StoreFrontRepository.EF.Test
{
    [Collection("Sequential")]
    public class StoreProductRepositoryEFTest: IDisposable
    {
        #region Public Constructors
        public StoreProductRepositoryEFTest()
        {
            this._sqlLoader = new SqlLoader();

            this._storeProductRepositoryEF = new StoreProductRepositoryEF(new LogService());

            try
            {
                this._sqlLoader.Setup("storefronteftest");
            }
            catch (Exception e)
            {
                this._sqlLoader.TearDown("storefronteftest");
            }
        }
        #endregion

        #region Private constructors
        private SqlLoader _sqlLoader { get; }

        private StoreProductRepositoryEF _storeProductRepositoryEF { get; }

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

            var result = _storeProductRepositoryEF.Insert(storeProduct);

            Assert.True(result);
        }

        [Fact]
        public void Insert_Exception()
        {
            StoreProduct storeProduct = null;

            Assert.Throws<ArgumentNullException>(() => _storeProductRepositoryEF.Insert(storeProduct));
        }

        [Fact]
        public void Delete_Success()
        {
            var result = _storeProductRepositoryEF.Delete(Guid.Parse("c25ef7e4-5641-40ac-b945-306fd3efc04b"), Guid.Parse("f1c2cf3e-b65f-4991-84a9-eca8dca3a08a"));

            Assert.True(result);
        }

        [Fact]
        public void Delete_Failure()
        {
            var result = _storeProductRepositoryEF.Delete(Guid.Parse("83fdc303-2b14-4436-a1dc-5a9dad9f959a"), Guid.Parse("b090ebcd-33e8-419e-9da7-d1b4d2b00114"));

            Assert.False(result);
        }

        [Fact]
        public void Delete_Exception()
        {
            Assert.Throws<ArgumentException>(() => _storeProductRepositoryEF.Delete(Guid.Empty, Guid.Empty));
        }

        #endregion

        #region Disposal

        public void Dispose()
        {
            _sqlLoader.TearDown("storefronteftest");
        }

        #endregion
    }
}
