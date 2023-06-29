using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSubstitute;
using StoreFront.API.Controllers;
using StoreFront.Common.Interfaces.Services;
using StoreFront.Common.Models;
using Xunit;

namespace StoreFront.API.Test
{
    [Collection("Sequential")]
    public class ProductControllerTest
    {
        #region Constructors
        public ProductControllerTest()
        {
            var productService = Substitute.For<IProductService>();

            Product productNull = null;

            this._successProduct = new Product()
            {
                ProductId = Guid.Parse("a18c4331-d606-4912-aa5c-f2179c6f8854")
            };

            this._failureProduct = new Product()
            {
                ProductId = Guid.Empty
            };

            this._productController = new ProductController(productService);

            #region Mocks
            productService.Get().Returns(new List<Product>() { new Product() });
            productService.GetSingle(Arg.Any<Guid>()).Returns(productNull);
            productService.Insert(this._successProduct).Returns(new ServiceResult<Product>() { IsSuccessful = true });
            productService.Insert(this._failureProduct).Returns(new ServiceResult<Product>() { IsSuccessful = false });
            productService.Update(this._successProduct).Returns(new ServiceResult<Product>() { IsSuccessful = true });
            productService.Update(this._failureProduct).Returns(new ServiceResult<Product>() { IsSuccessful = false });
            productService.Delete(Arg.Any<Guid>()).Returns(new ServiceResult<Product> { IsSuccessful = false });
            productService.Delete(Guid.Parse("7265b27e-e38c-42ac-943f-0bb8acbce659")).Returns(new ServiceResult<Product>() { IsSuccessful = true });
            productService.ProductSearch(Arg.Any<string>()).Returns(new List<Product>());
            productService.ProductSearch("Paper").Returns(new List<Product>() { new Product() });
            #endregion
        }
        #endregion

        #region Private Properties
        private ProductController _productController { get; }

        private Product _successProduct { get; }

        private Product _failureProduct { get; }

        #endregion

        #region Tests

        [Fact]
        public void Get_Success()
        {
            var result = (ObjectResult)this._productController
                .Get();

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void GetProductsNotMatchingStoreId_Failure()
        {
            var result = (StatusCodeResult)this._productController
                .GetProductsNotMatchingStoreId(Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GetProductsByStoreId_Failure()
        {
            var result = (StatusCodeResult) this._productController
                .GetProductsByStoreId(Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GetSingle_Failure1()
        {
            var result = (StatusCodeResult)this._productController
                .GetSingle(Guid.Parse("c7b4a247-8510-4afb-925c-064aeeda361e"));

            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public void GetSingle_Failure2()
        {
            var result = (StatusCodeResult)this._productController
                .GetSingle(Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Insert_Success()
        {
            var result = (ObjectResult)this._productController
                .Post(this._successProduct);

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void Insert_Failure1()
        {
            var result = (ObjectResult)this._productController
                .Post(this._failureProduct);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Insert_Failure2()
        {
            var result = (StatusCodeResult)this._productController
                .Post(null);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_Success()
        {
            var result = (ObjectResult)this._productController
                .Put(this._successProduct);

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Update_Failure1()
        {
            var result = (ObjectResult)this._productController
                .Put(this._failureProduct);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_Failure2()
        {
            var result = (StatusCodeResult)this._productController
                .Put(null);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Delete_Success()
        {
            var result = (ObjectResult)this._productController
                .Delete(Guid.Parse("7265b27e-e38c-42ac-943f-0bb8acbce659"));

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Delete_Failure1()
        {
            var result = (StatusCodeResult)this._productController
                .Delete(Guid.Empty);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Delete_Failure2()
        {
            var result = (ObjectResult)this._productController
                .Delete(Guid.NewGuid());

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void ProductSearch_Success()
        {
            var result = (ObjectResult)this._productController
                .ProductSearch("Paper");

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void ProductSearch_Failure()
        {
            var result = (StatusCodeResult)this._productController
                .ProductSearch("Something that isn't paper");

            Assert.Equal(204, result.StatusCode);
        }
        #endregion
    }
}
