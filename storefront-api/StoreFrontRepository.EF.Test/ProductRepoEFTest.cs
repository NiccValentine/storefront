using System;
using SqlTestPrep;
using Xunit;
using StoreFront.Common.Models;
using StoreFront.EF.Repository;
using StoreFront.Common.Logging;

namespace StoreFrontRepository.EF.Test
{
    [Collection("Sequential")]
    public class ProductRepositoryEFTest : IDisposable
    {
        #region Public Constructors
        public ProductRepositoryEFTest()
        {
            this._sqlLoader = new SqlLoader();

            this._productRepositoryEF = new ProductRepositoryEF(new LogService());

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

        private ProductRepositoryEF _productRepositoryEF { get; }

        #endregion

        #region Tests
        [Fact]
        public void Get_Success()
        {
            var result = this._productRepositoryEF.Get();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetProductsNotMatchingStoreId_Success()
        {
            var result = this._productRepositoryEF.GetProductsNotMatchingStoreId(Guid.Parse("c25ef7e4-5641-40ac-b945-306fd3efc04b"));

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetProductsNotMatchingStoreId_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._productRepositoryEF.GetProductsNotMatchingStoreId(Guid.Empty));
        }

        [Fact]
        public void GetProductsByStoreId_Success()
        {
            var result = this._productRepositoryEF.GetProductsByStoreId(Guid.Parse("ff0c12cc-c79a-428d-aafc-7a93970b01cd"));

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void GetProductsByStoreId_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._productRepositoryEF.GetProductsByStoreId(Guid.Empty));
        }

        [Fact]
        public void GetSingle_Success()
        {
            var result = this._productRepositoryEF.GetSingle(Guid.Parse("a7c29719-d5ed-400c-ae26-be578ee22b38"));

            Assert.NotNull(result);
        }

        [Fact]
        public void GetSingle_Failure()
        {
            var result = this._productRepositoryEF.GetSingle(Guid.Parse("58c2e109-d57b-4832-b477-eba01ebdc0ae"));

            Assert.Null(result);
        }

        [Fact]
        public void GetSingle_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._productRepositoryEF.GetSingle(Guid.Empty));
        }

        [Fact]
        public void Insert_Success()
        {
            var product = new Product()
            {
                ProductId = Guid.Parse("f25d84d8-44df-4af9-b796-aea3e0fc03dd"),
                ProductName = "Space Bag Storeage Packs",
                ProductDescription = "Vacuum sealed bags for garments and stuffed toys"
            };

            var result = this._productRepositoryEF.Insert(product);

            Assert.True(result);
        }

        [Fact]
        public void Insert_Exception()
        {
            Product product = null;

            Assert.Throws<ArgumentNullException>(() => this._productRepositoryEF.Insert(product));
        }

        [Fact]
        public void Update_Success()
        {
            var product = new Product()
            {
                ProductId = Guid.Parse("f1c2cf3e-b65f-4991-84a9-eca8dca3a08a"),
                ProductName = "Nintendo 64",
                ProductDescription = "A classic gaming console from Nintendo"
            };

            var result = this._productRepositoryEF.Update(product);

            Assert.True(result);
        }

        [Fact]
        public void Update_Exception()
        {
            Product product = null;

            Assert.Throws<ArgumentNullException>(() => this._productRepositoryEF.Update(product));
        }

        [Fact]
        public void Delete_Success()
        {
            var result = this._productRepositoryEF.Delete(Guid.Parse("f0743f41-4c22-432a-9501-1855bdc5f887"));

            Assert.True(result);
        }

        [Fact]
        public void Delete_Failure()
        {
            var result = this._productRepositoryEF.Delete(Guid.Parse("cac0d3d6-8558-401d-a94a-5b96dc409556"));

            Assert.False(result);
        }

        [Fact]
        public void Delete_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._productRepositoryEF.Delete(Guid.Empty));
        }

        [Fact]
        public void ProductSearch_Success1()
        {
            var result = this._productRepositoryEF.ProductSearch("");

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void ProductSearch_Success2()
        {
            var result = this._productRepositoryEF.ProductSearch("product2");

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void ProductSearch_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => this._productRepositoryEF.ProductSearch(null));
        }
        #endregion

        #region Disposal
        public void Dispose()
        {
            this._sqlLoader.TearDown("storefronteftest");
        }
        #endregion
    }
}
