namespace StoreFront.Service
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using Common.Interfaces.Repositories;
    using Common.Interfaces.Services;
    using Common.Models;
    using FluentValidation;

    public class ProductService : IProductService
    {
        #region Constructors

        public ProductService(IProductRepository productRepository)
        {
            this._productRepository = productRepository;

            this._productValidator = new ProductValidator();
        }

        #endregion

        #region Private Properties

        private IProductRepository _productRepository { get; }

        private ProductValidator _productValidator { get; }

        #endregion

        #region Public Methods

        public List<Product> Get()
        {
            var products = this._productRepository.Get();

            return products;
        }

        public List<Product> GetProductsNotMatchingStoreId(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            var products = this._productRepository.GetProductsNotMatchingStoreId(storeId);

            return products;
        }

        public List<Product> GetProductsByStoreId(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            var products = this._productRepository.GetProductsByStoreId(storeId);

            return products;
        }

        public Product GetSingle(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException(nameof(productId));
            }

            var product = this._productRepository.GetSingle(productId);

            return product;
        }

        public ServiceResult<Product> Insert(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var serviceResult = this._productValidator.ValidateProduct(product);

            if (!serviceResult.IsValid)
            {
                return serviceResult;
            }

            serviceResult.IsSuccessful = this._productRepository.Insert(product);

            return serviceResult;
        }

        public ServiceResult<Product> Update(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var serviceResult = this._productValidator.ValidateProduct(product);

            if (!serviceResult.IsValid)
            {
                return serviceResult;
            }

            serviceResult.IsSuccessful = this._productRepository.Update(product);

            return serviceResult;
        }

        public ServiceResult<Product> Delete(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException(nameof(productId));
            }

            var serviceResult = new ServiceResult<Product>();

            serviceResult.IsSuccessful = this._productRepository.Delete(productId);

            return serviceResult;
        }

        public List<Product> ProductSearch(string productName)
        {
            if (productName == null)
            {
                throw new ArgumentNullException(nameof(productName));
            }

            var products = this._productRepository.ProductSearch(productName);

            return products;
        }


        #endregion

        #region Private Classes

        private class ProductValidator : AbstractValidator<Product>
        {
            public ProductValidator()
            {
                this.RuleFor(p => p.ProductId).NotNull().NotEmpty();
                this.RuleFor(p => p.ProductName).NotNull().NotEmpty().MaximumLength(50);
                this.RuleFor(p => p.ProductDescription).NotNull().NotEmpty().MaximumLength(2000);
            }

            public ServiceResult<Product> ValidateProduct(Product product)
            {
                var serviceResult = new ServiceResult<Product>()
                {
                    Object = product
                };

                var result = this.Validate(product);

                serviceResult.IsValid = result.IsValid;

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        serviceResult.Messages.Add(new Message() { MessageText = error.ErrorMessage, FieldName = error.PropertyName });
                    }
                }

                return serviceResult;
            }
        }

        #endregion

    }
}
