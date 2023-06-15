namespace StoreFront.Service
{
    using System;
    using System.Collections.Generic;
    using Common.Interfaces.Repositories;
    using Common.Interfaces.Services;
    using Common.Models;
    using FluentValidation;

    public class StoreProductService : IStoreProductService
    {
        #region Constructors

        public StoreProductService(IStoreProductRepository storeProductRepository)
        {
            this._storeProductRepository = storeProductRepository;

            this._storeProductValidator = new StoreProductValidator();
        }

        #endregion

        #region Private Properties

        private IStoreProductRepository _storeProductRepository { get; }

        private StoreProductValidator _storeProductValidator { get; }

        #endregion

        #region Public Methods

        public ServiceResult<StoreProduct> Insert(StoreProduct storeProduct)
        {
            if (storeProduct == null)
            {
                throw new ArgumentNullException(nameof(storeProduct));
            }

            var serviceResult = this._storeProductValidator.ValidateStoreProduct(storeProduct);

            if (!serviceResult.IsValid)
            {
                return serviceResult;
            }

            serviceResult.IsSuccessful = this._storeProductRepository.Insert(storeProduct);

            return serviceResult;
        }

        public ServiceResult<StoreProduct> Delete(Guid storeId, Guid productId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            if (productId == Guid.Empty)
            {
                throw new ArgumentException(nameof(productId));
            }

            var serviceResult = new ServiceResult<StoreProduct>();

            serviceResult.IsSuccessful = this._storeProductRepository.Delete(storeId, productId);

            return serviceResult;
        }
        #endregion

        #region Private Classes

        private class StoreProductValidator : AbstractValidator<StoreProduct>
        {
            public StoreProductValidator()
            {
                this.RuleFor(sp => sp.StoreId).NotNull().NotEmpty();
                this.RuleFor(sp => sp.ProductId).NotNull().NotEmpty();
            }

            public ServiceResult<StoreProduct> ValidateStoreProduct(StoreProduct storeProduct)
            {
                var serviceResult = new ServiceResult<StoreProduct>()
                {
                    Object = storeProduct
                };

                var result = this.Validate(storeProduct);

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




