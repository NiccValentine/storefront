namespace StoreFront.Service
{
    using System;
    using System.Collections.Generic;
    using Common.Interfaces.Repositories;
    using Common.Interfaces.Services;
    using Common.Models;
    using FluentValidation;
    using StoreFront.Common.Interfaces.Logging;

    public class StoreProductService : IStoreProductService
    {
        #region Constructors

        public StoreProductService(IStoreProductRepository storeProductRepository, ILogService logService)
        {
            this._storeProductRepository = storeProductRepository;

            this._logService = logService;

            this._storeProductValidator = new StoreProductValidator();
        }

        #endregion

        #region Private Properties

        private IStoreProductRepository _storeProductRepository { get; }

        private ILogService _logService { get; }

        private StoreProductValidator _storeProductValidator { get; }

        #endregion

        #region Public Methods

        public ServiceResult<StoreProduct> Insert(StoreProduct storeProduct)
        {
            this._logService.Debug("StoreProductService.Insert called");

            if (storeProduct == null)
            {
                this._logService.Warn("StoreProductService.Insert storeProduct null");

                throw new ArgumentNullException(nameof(storeProduct));
            }

            var serviceResult = this._storeProductValidator.ValidateStoreProduct(storeProduct);

            if (!serviceResult.IsValid)
            {
                this._logService.Warn("StoreService.Insert has encountered validation errors");

                return serviceResult;
            }

            serviceResult.IsSuccessful = this._storeProductRepository.Insert(storeProduct);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("StoreProductService.Insert has successfully inserted data");
            }
            else
            {
                this._logService.Trace("StoreProductService.Insert has not inserted data");
            }

            return serviceResult;
        }

        public ServiceResult<StoreProduct> Delete(Guid storeId, Guid productId)
        {
            this._logService.Debug("StoreProductService.Delete called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreProductService.Delete storeId not present");

                throw new ArgumentException(nameof(storeId));
            }

            if (productId == Guid.Empty)
            {
                this._logService.Warn("StoreProductService.Delete productId not present");

                throw new ArgumentException(nameof(productId));
            }

            var serviceResult = new ServiceResult<StoreProduct>();

            serviceResult.IsSuccessful = this._storeProductRepository.Delete(storeId, productId);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("StoreProductService.Delete has successfully removed data");
            }
            else
            {
                this._logService.Trace("StoreProductService.Delete has not removed data");
            }

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




