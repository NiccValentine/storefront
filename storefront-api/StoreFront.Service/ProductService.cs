namespace StoreFront.Service
{
    using System;
    using System.Collections.Generic;
    using Common.Interfaces.Repositories;
    using Common.Interfaces.Services;
    using Common.Models;
    using FluentValidation;
    using StoreFront.Common.Interfaces.Logging;

    public class ProductService : IProductService
    {
        #region Constructors

        public ProductService(IProductRepository productRepository, ILogService logService)
        {
            this._productRepository = productRepository;

            this._logService = logService;

            this._productValidator = new ProductValidator();
        }

        #endregion

        #region Private Properties

        private IProductRepository _productRepository { get; }

        private ILogService _logService { get; }

        private ProductValidator _productValidator { get; }

        #endregion

        #region Public Methods

        public List<Product> Get()
        {
            this._logService.Debug("ProductService.Get called");

            var products = this._productRepository.Get();

            this._logService.Trace("ProductService.Get returned {0} result(s)", products.Count);

            return products;
        }

        public List<Product> GetProductsNotMatchingStoreId(Guid storeId)
        {
            this._logService.Debug("ProductService.GetProductsNotMatchingStoreId called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("ProductService.GetProductsNotMatchingStoreId storeId not present");

                throw new ArgumentException(nameof(storeId));
            }

            var products = this._productRepository.GetProductsNotMatchingStoreId(storeId);

            this._logService.Debug("ProductService.GetProductsNotMatchingStoreId has returned {0} result(s)", products.Count);

            return products;
        }

        public List<Product> GetProductsByStoreId(Guid storeId)
        {
            this._logService.Debug("ProductService.GetProductsByStoreId called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("ProductService.GetProductsByStoreId storeId not present");

                throw new ArgumentException(nameof(storeId));
            }

            var products = this._productRepository.GetProductsByStoreId(storeId);

            this._logService.Trace("ProductService.GetProductsByStoreId returned {0} result(s)", products.Count);

            return products;
        }

        public Product GetSingle(Guid productId)
        {
            this._logService.Debug("ProductService.GetSingle called");

            if (productId == Guid.Empty)
            {
                this._logService.Trace("ProductService.GetSingle returned 0 results");

                throw new ArgumentException(nameof(productId));
            }

            var product = this._productRepository.GetSingle(productId);

            this._logService.Trace("ProductService.GetSingle returned 1 result");

            return product;
        }

        public ServiceResult<Product> Insert(Product product)
        {
            this._logService.Debug("ProductService.Insert called");

            if (product == null)
            {
                this._logService.Warn("ProductService.Insert product is null");

                throw new ArgumentNullException(nameof(product));
            }

            var serviceResult = this._productValidator.ValidateProduct(product);

            if (!serviceResult.IsValid)
            {
                this._logService.Warn("ProductService.Insert encountered a validation error");

                return serviceResult;
            }

            serviceResult.IsSuccessful = this._productRepository.Insert(product);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("ProductService.Insert has successfully inserted data");
            }
            else
            {
                this._logService.Trace("ProductService.Delete has not inserted data");
            }

            return serviceResult;
        }

        public ServiceResult<Product> Update(Product product)
        {
            this._logService.Debug("ProductService.Update called");

            if (product == null)
            {
                this._logService.Warn("ProductService.Update product is null");

                throw new ArgumentNullException(nameof(product));
            }

            var serviceResult = this._productValidator.ValidateProduct(product);

            if (!serviceResult.IsValid)
            {
                this._logService.Warn("ProductService.Update has encountered a validation error");

                return serviceResult;
            }

            serviceResult.IsSuccessful = this._productRepository.Update(product);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("ProductService.Update has altered the database");
            }
            else
            {
                this._logService.Trace("ProductService.Update has not altered the database");
            }

            return serviceResult;
        }

        public ServiceResult<Product> Delete(Guid productId)
        {
            this._logService.Debug("ProductService.Delete called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("ProductService.Delete productId not present");

                throw new ArgumentException(nameof(productId));
            }

            var serviceResult = new ServiceResult<Product>();

            serviceResult.IsSuccessful = this._productRepository.Delete(productId);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("ProductService.Delete has successfully removed data");
            }
            else
            {
                this._logService.Trace("ProductService.Delete has not removed data");
            }

            return serviceResult;
        }

        public List<Product> ProductSearch(string productName)
        {
            this._logService.Debug("ProductService.ProductSearch called");

            if (productName == null)
            {
                this._logService.Warn("ProductService.ProductSearch productName is null");

                throw new ArgumentNullException(nameof(productName));
            }

            var products = this._productRepository.ProductSearch(productName);

            this._logService.Trace("ProductService.ProductSearch returned {0} result(s)", products.Count);

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
