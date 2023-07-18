namespace StoreFront.Service
{
    using System;
    using System.Collections.Generic;
    using Common.Interfaces.Repositories;
    using Common.Interfaces.Services;
    using Common.Models;
    using FluentValidation;
    using StoreFront.Common.Interfaces.Logging;

    public class StoreService : IStoreService
    {
        #region Constructors

        public StoreService(IStoreRepository storeRepository, ILogService logService)
        {
            this._storeRepository = storeRepository;

            this._logService = logService;

            this._storeValidator = new StoreValidator();
        }

        #endregion

        #region Private Properties

        private IStoreRepository _storeRepository { get; }

        private ILogService _logService { get; }

        private StoreValidator _storeValidator { get; }

        #endregion

        #region Public Methods

        public List<Store> Get()
        {
            this._logService.Debug("StoreService.Get called");

            var stores = this._storeRepository.Get();

            this._logService.Debug("StoreService.Get returned {0} result(s)", stores.Count);

            return stores;
        }

        public List<Store> GetStoresByProductId(Guid productId)
        {
            this._logService.Debug("StoreService.GetStoresByProductId called");

            if (productId == Guid.Empty)
            {
                this._logService.Warn("StoreService.GetStoresByProductId productId is not present");

                throw new ArgumentException(nameof(productId));
            }

            var stores = this._storeRepository.GetStoresByProductId(productId);

            this._logService.Trace("StoreService.GetStoresByProductId returned {0} result(s)", stores.Count);

            return stores;
        }

        public Store GetSingle(Guid storeId)
        {
            this._logService.Debug("StoreService.GetSingle called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreService.GetSingle storeId is not present");

                throw new ArgumentException(nameof(storeId));
            }

            var store = this._storeRepository.GetSingle(storeId);

            this._logService.Trace("StoreService.GetSingle has returned 1 result");

            return store;
        }

        public ServiceResult<Store> Insert(Store store)
        {
            this._logService.Debug("StoreService.Insert called");

            if (store == null)
            {
                this._logService.Warn("StoreService.Insert store is null");

                throw new ArgumentNullException(nameof(store));
            }

            var serviceResult = this._storeValidator.ValidateStore(store);

            if (!serviceResult.IsValid)
            {
                this._logService.Warn("StoreService.Insert encountered a validation error");

                return serviceResult;
            }

            serviceResult.IsSuccessful = this._storeRepository.Insert(store);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("StoreService.Insert has successfully inserted data");
            }
            else
            {
                this._logService.Trace("StoreService.Insert has not inserted data");
            }

            return serviceResult;
        }

        public ServiceResult<Store> Update(Store store)
        {
            this._logService.Debug("StoreService.Update called");

            if (store == null)
            {
                this._logService.Warn("StoreService.Update product is null");

                throw new ArgumentNullException(nameof(store));
            }

            var serviceResult = this._storeValidator.ValidateStore(store);

            if (!serviceResult.IsValid)
            {
                this._logService.Warn("StoreService.Update has encountered a validation error");

                return serviceResult;
            }

            serviceResult.IsSuccessful = this._storeRepository.Update(store);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("StoreService.Update has altered the database");
            }
            else
            {
                this._logService.Trace("StoreService.Update has not altered the database");
            }

            return serviceResult;
        }

        public ServiceResult<Store> Delete(Guid storeId)
        {
            this._logService.Debug("StoreService.Delete called");

            if (storeId == Guid.Empty)
            {
                this._logService.Warn("StoreService.Delete storeId not present");

                throw new ArgumentException(nameof(storeId));
            }

            var serviceResult = new ServiceResult<Store>();

            serviceResult.IsSuccessful = this._storeRepository.Delete(storeId);

            if (serviceResult.IsSuccessful)
            {
                this._logService.Trace("StoreService.Delete has successfully removed data");
            }
            else
            {
                this._logService.Trace("StoreService.Delete has not removed data");
            }

            return serviceResult;
        }

        public List<Store> StoreSearch(string storeName)
        {
            this._logService.Debug("StoreService.StoreSearch called");

            if (storeName == null)
            {
                this._logService.Warn("StoreService.StoreSearch storeName is null");

                throw new ArgumentNullException(nameof(storeName));
            }

            var stores = this._storeRepository.StoreSearch(storeName);

            this._logService.Trace("StoreService.StoreSearch returned {0} result(s)", stores.Count);

            return stores;
        }
        #endregion


        #region Private Classes
        private class StoreValidator : AbstractValidator<Store>
        {
            public StoreValidator()
            {
                this.RuleFor(s => s.StoreId).NotNull().NotEmpty();
                this.RuleFor(s => s.StoreName).NotNull().NotEmpty().MaximumLength(50);
                this.RuleFor(s => s.StoreDescription).NotNull().NotEmpty().MaximumLength(2000);
            }

            public ServiceResult<Store> ValidateStore(Store store)
            {
                var serviceResult = new ServiceResult<Store>()
                {
                    Object = store
                };

                var result = this.Validate(store);

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
