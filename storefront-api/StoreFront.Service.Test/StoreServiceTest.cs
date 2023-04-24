﻿using System;
using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.Service;
using NSubstitute;
using Xunit;


namespace StoreFront.Service.Test
{
    public class StoreServiceTest
    {
        public StoreServiceTest() 
        {
            var testStore = new Store();

            Store storeNull = null;

            var storeRepository = Substitute.For<IStoreRepository>();

            storeRepository.GetSingle(Arg.Any<Guid>()).Returns(storeNull);
            storeRepository.GetSingle(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f")).Returns(testStore);
            storeRepository.Insert(Arg.Any<Store>()).Returns(true);
            storeRepository.Update(Arg.Any<Store>()).Returns(true);
            storeRepository.Delete(Arg.Any<Guid>()).Returns(false);
            storeRepository.Delete(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f")).Returns(true);

            this._storeService = new StoreService(storeRepository);
        }

        private StoreService _storeService { get; }

        [Fact]
        public void GetSingle_Success()
        {
            var result = this._storeService.GetSingle(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f"));

            Assert.NotNull(result);
        }

        [Fact]
        public void GetSingle_Failure()
        {
            var result = this._storeService.GetSingle(Guid.Parse("d6cc3820-3e00-458a-a464-da9984f38480"));

            Assert.Null(result);
        }

        [Fact]
        public void GetSingle_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._storeService.GetSingle(Guid.Empty));
        }

        [Fact]
        public void Insert_Success()
        {
            var store = new Store()
            {
                StoreId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                StoreName = "Circuit City",
                StoreDescription = "Discontinued tech hub"
            };

            var result = this._storeService.Insert(store);

            Assert.True(result.IsSuccessful);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validation_StoreId_Failure()
        {
            var store = new Store()
            {
                StoreId = Guid.Empty,
                StoreName = "Circuit City"
            };

            var result = this._storeService.Insert(store);

            Assert.False(result.IsSuccessful);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validation_StoreName_Failure()
        {
            var store = new Store()
            {
                StoreId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                StoreName = "",
                StoreDescription = "Somesuch about this store that was hypothetically forgotten to be named"
            };

            var result = this._storeService.Insert(store);

            Assert.False(result.IsSuccessful);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validation_StoreDescription_Failure()
        {
            var store = new Store()
            {
                StoreId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                StoreName = "Bass Pro Shops",
                StoreDescription = ""
            };

            var result = this._storeService.Insert(store);

            Assert.False(result.IsSuccessful);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Update_Success()
        {
            var store = new Store()
            {
                StoreId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                StoreName = "Circuit City",
                StoreDescription = "404 store not found"
            };

            var result = this._storeService.Update(store);

            Assert.True(result.IsSuccessful);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Delete_Success()
        {
            var result = this._storeService.Delete(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f"));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Delete_Failure()
        {
            var result = this._storeService.Delete(Guid.Parse("d6cc3820-3e00-458a-a464-da9984f38480"));

            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void Delete_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._storeService.Delete(Guid.Empty));
        }
    }
}