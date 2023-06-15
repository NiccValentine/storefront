using System;
using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.Service;
using NSubstitute;
using Xunit;

namespace StoreFront.Service.Test
{
    public class StoreProductServiceTest
    {
        public StoreProductServiceTest()
        {
            var testStoreProduct = new StoreProduct();

            StoreProduct storeProductNull = null;

            var storeProductRepository = Substitute.For<IStoreProductRepository>();

            storeProductRepository.Insert(Arg.Any<StoreProduct>()).Returns(true);
            storeProductRepository.Delete(Arg.Any<Guid>(), Arg.Any<Guid>()).Returns(false);
            storeProductRepository.Delete(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f"), Guid.Parse("de208a9c-19a9-48f2-a76f-397638f8685f")).Returns(true, true);

            this._storeProductService = new StoreProductService(storeProductRepository);
        }

        private StoreProductService _storeProductService { get; }

        [Fact]
        public void Insert_Success()
        {
            var storeProduct = new StoreProduct()
            {
                StoreId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                ProductId = Guid.Parse("d6cc3820-3e00-458a-a464-da9984f38480")
            };

            var result = this._storeProductService.Insert(storeProduct);

            Assert.True(result.IsSuccessful);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validation_StoreId_Failure()
        {
            var storeProduct = new StoreProduct()
            {
                StoreId = Guid.Empty,
                ProductId = Guid.Parse("d6cc3820-3e00-458a-a464-da9984f38480")
            };

            var result = this._storeProductService.Insert(storeProduct);

            Assert.False(result.IsSuccessful);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validation_ProductId_Failure()
        {
            var storeProduct = new StoreProduct()
            {
                StoreId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                ProductId = Guid.Empty
            };

            var result = this._storeProductService.Insert(storeProduct);

            Assert.False(result.IsSuccessful);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Delete_Success()
        {
            var result = this._storeProductService.Delete(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f"), Guid.Parse("de208a9c-19a9-48f2-a76f-397638f8685f"));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Delete_Exception_StoreId()
        {
            Assert.Throws<ArgumentException>(() => this._storeProductService.Delete(Guid.Empty, Guid.NewGuid()));
        }

        [Fact]
        public void Delete_Exception_ProductId()
        {
            Assert.Throws<ArgumentException>(() => this._storeProductService.Delete(Guid.NewGuid(), Guid.Empty));
        }
    }
}
