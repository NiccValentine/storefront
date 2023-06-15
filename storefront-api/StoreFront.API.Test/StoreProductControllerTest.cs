using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using StoreFront.API.Controllers;
using StoreFront.Common.Interfaces.Services;
using StoreFront.Common.Models;
using Xunit;

namespace StoreFront.API.Test
{
    [Collection("Sequential")]
    public class StoreProductControllerTest
    {
        #region Constructors
        public StoreProductControllerTest()
        {
            var storeProductService = Substitute.For<IStoreProductService>();

            StoreProduct storeProductNull = null;

            this._storeProductController = new StoreProductController(storeProductService);

            this._successStoreProduct = new StoreProduct()
            {
                StoreId = Guid.Parse("7cebc427-19fc-4bc1-ae0d-46b6dd778ad4"),
                ProductId = Guid.Parse("c07e9e71-206b-495a-9f90-cab6b0c19563")
            };

            this._failureStoreProduct = new StoreProduct()
            {
                StoreId = Guid.Empty,
                ProductId = Guid.Empty
            };

            #region Mocks
            storeProductService.Insert(this._successStoreProduct).Returns(new ServiceResult<StoreProduct>() { IsSuccessful = true });
            storeProductService.Insert(this._failureStoreProduct).Returns(new ServiceResult<StoreProduct>() { IsSuccessful = false });
            storeProductService.Delete(Arg.Any<Guid>(), Arg.Any<Guid>()).Returns(new ServiceResult<StoreProduct>() { IsSuccessful = false });
            storeProductService.Delete(Guid.Parse("7f8e89b7-ce78-4770-9de7-dbc2e9a1f8c5"), Guid.Parse("ef7fcbd7-40f3-4c57-a54b-4a42917177be")).Returns(new ServiceResult<StoreProduct>() { IsSuccessful = true });
            #endregion
        }
        #endregion

        #region Private Properties
        private StoreProductController _storeProductController { get; }
        
        private StoreProduct _successStoreProduct { get; }

        private StoreProduct _failureStoreProduct { get; }
        #endregion

        #region Tests
        [Fact]
        public void Insert_Success()
        {
            var result = (ObjectResult)this._storeProductController
                .Post(this._successStoreProduct);

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void Insert_Failure1()
        {
            var result = (ObjectResult)this._storeProductController
                .Post(this._failureStoreProduct);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Insert_Failure2()
        {
            var result = (StatusCodeResult)this._storeProductController
                .Post(null);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Delete_Success()
        {
            var result = (ObjectResult)this._storeProductController
                .Delete(Guid.Parse("7f8e89b7-ce78-4770-9de7-dbc2e9a1f8c5"), Guid.Parse("ef7fcbd7-40f3-4c57-a54b-4a42917177be"));

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Delete_Failure1()
        {
            var result = (StatusCodeResult)this._storeProductController
                .Delete(Guid.Empty, Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Delete_Failure2()
        {
            var result = (StatusCodeResult)this._storeProductController
                .Delete(Guid.Empty, Guid.NewGuid());

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Delete_Failure3()
        {
            var result = (StatusCodeResult)this._storeProductController
                .Delete(Guid.NewGuid(), Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }
        #endregion
    }
}
