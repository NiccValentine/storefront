using System;
using StoreFront.Common.Interfaces.Repositories;
using StoreFront.Common.Models;
using StoreFront.Service;
using NSubstitute;
using Xunit;

namespace StoreFront.Service.Test
{
    public class ProductServiceTest
    {
        public ProductServiceTest() 
        {
            var testProduct = new Product();

            Product productNull = null;

            var productRepository = Substitute.For<IProductRepository>();

            productRepository.GetSingle(Arg.Any<Guid>()).Returns(productNull);
            productRepository.GetSingle(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f")).Returns(testProduct);
            productRepository.Insert(Arg.Any<Product>()).Returns(true);
            productRepository.Update(Arg.Any<Product>()).Returns(true);
            productRepository.Delete(Arg.Any<Guid>()).Returns(false);
            productRepository.Delete(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f")).Returns(true);

            this._productService = new ProductService(productRepository);
        }

        public ProductService _productService { get; }

        [Fact]
        public void GetSingle_Success()
        {
            var result = this._productService.GetSingle(Guid.Parse("fcb358b9-7044-441e-bc41-9f5d5a4e421f"));

            Assert.NotNull(result);
        }

        [Fact]
        public void GetSingle_Failure()
        {
            var result = this._productService.GetSingle(Guid.Parse("d6cc3820-3e00-458a-a464-da9984f38480"));

            Assert.Null(result);
        }

        [Fact]
        public void GetSingle_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._productService.GetSingle(Guid.Empty));
        }

        [Fact]
        public void Insert_Success()
        {
            var product = new Product()
            {
                ProductId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                ProductName = "XBox One",
                ProductDescription = "Microsoft's newest fastest console!"
            };

            var result = this._productService.Insert(product);

            Assert.True(result.IsSuccessful);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validation_ProductId_Failure()
        {
            var product = new Product()
            {
                ProductId = Guid.Empty,
                ProductName = "XBox One",
                ProductDescription = "Microsoft's newest fastest console!"
            };
        }

        [Fact]
        public void Validation_ProductName_Failure()
        {
            var product = new Product()
            {
                ProductId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                ProductName = "",
                ProductDescription = "Microsoft's newest fastest console!"
            };

            var result = this._productService.Insert(product);

            Assert.False(result.IsSuccessful);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validation_ProductDescription_Failure()
        {
            var product = new Product()
            {
                ProductId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                ProductName = "XBox One",
                ProductDescription = ""
            };

            var result = this._productService.Insert(product);

            Assert.False(result.IsSuccessful);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Update_Success()
        {
            var product = new Product()
            {
                ProductId = Guid.Parse("3513405d-4b44-4610-87a5-f0664eda7a4c"),
                ProductName = "Maserati Ghibli",
                ProductDescription = "Fast Italian supercar"
            };

            var result = this._productService.Update(product);

            Assert.True(result.IsSuccessful);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Delete_Success()
        {
            Assert.Throws<ArgumentException>(() => this._productService.Delete(Guid.Empty));
        }

        [Fact]
        public void Delete_Failure()
        {
            var result = this._productService.Delete(Guid.Parse("d6cc3820-3e00-458a-a464-da9984f38480"));

            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void Delete_Exception()
        {
            Assert.Throws<ArgumentException>(() => this._productService.Delete(Guid.Empty));
        }
    }
}
