using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSubstitute;
using StoreFront.API.Controllers;
using StoreFront.Common.Interfaces.Services;
using StoreFront.Common.Models;
using StoreFront.Service;
using Xunit;

namespace StoreFront.API.Test
{
    [Collection("Sequential")]
    public class StoreControllerTest
    {
        #region Constructors
        public StoreControllerTest()
        {
            var storeService = Substitute.For<IStoreService>();

            Store storeNull = null;

            this._storeController = new StoreController(storeService);

            this._successStore = new Store()
            {
                StoreId = Guid.Parse("8216fc87-ae51-44e4-84c9-5570bb1aabaf")
            };

            this._failureStore = new Store()
            {
                StoreId = Guid.Empty
            };

            #region Mocks
            storeService.Get().Returns(new List<Store>() { new Store() });
            storeService.GetSingle(Arg.Any<Guid>()).Returns(storeNull);
            storeService.Insert(this._successStore).Returns(new ServiceResult<Store>() { IsSuccessful = true });
            storeService.Insert(this._failureStore).Returns(new ServiceResult<Store>() { IsSuccessful = false });
            storeService.Update(this._successStore).Returns(new ServiceResult<Store>() { IsSuccessful = true });
            storeService.Update(this._failureStore).Returns(new ServiceResult<Store>() { IsSuccessful = false });
            storeService.Delete(Arg.Any<Guid>()).Returns(new ServiceResult<Store> { IsSuccessful = false });
            storeService.Delete(Guid.Parse("7ee41cc7-0121-4f2a-a55e-c5a5ad074535")).Returns(new ServiceResult<Store> { IsSuccessful = true });
            storeService.StoreSearch(Arg.Any<string>()).Returns(new List<Store>());
            storeService.StoreSearch("Papa Johns").Returns(new List<Store>() { new Store() });
            #endregion
        }
        #endregion

        #region Private Properties
        private StoreController _storeController { get; }

        private Store _successStore { get; }

        private Store _failureStore { get; }
        #endregion

        #region Tests
        [Fact]
        public void Get_Success()
        {
            var result = (ObjectResult)this._storeController
                .Get();

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void GetStoresByProductId_Failure()
        {
            var result = (StatusCodeResult)this._storeController
                .GetStoresByProductId(Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GetSingle_Failure1()
        {
            var result = (StatusCodeResult)this._storeController
                .GetSingle(Guid.Parse("b3a382c1-6548-41f9-9f67-4bb8ae71ea56"));

            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public void GetSingle_Failure2()
        {
            var result = (StatusCodeResult)this._storeController
                .GetSingle(Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Insert_Success()
        {
            var result = (ObjectResult)this._storeController
                .Post(this._successStore);

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void Insert_Failure1()
        {
            var result = (ObjectResult)this._storeController
                .Post(this._failureStore);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Insert_Failure2()
        {
            var result = (StatusCodeResult)this._storeController
                .Post(null);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_Success()
        {
            var result = (ObjectResult)this._storeController
                .Put(this._successStore);

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Update_Failure1()
        {
            var result = (ObjectResult)this._storeController
                .Put(this._failureStore);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_Failure2()
        {
            var result = (StatusCodeResult)this._storeController
                .Put(null);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Delete_Success()
        {
            var result = (ObjectResult)this._storeController
                .Delete(Guid.Parse("7ee41cc7-0121-4f2a-a55e-c5a5ad074535"));

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Delete_Failure1()
        {
            var result = (StatusCodeResult)this._storeController
                .Delete(Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Delete_Failure2()
        {
            var result = (ObjectResult)this._storeController
                .Delete(Guid.NewGuid());

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void StoreSearch_Success()
        {
            var result = (ObjectResult)this._storeController
                .StoreSearch("Papa Johns");

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void StoreSearch_Failure()
        {
            var result = (StatusCodeResult)this._storeController
                .StoreSearch("Pizza Hut");

            Assert.Equal(204, result.StatusCode);
        }
        #endregion
    }
}
