namespace StoreFront.Service
{
    using System;
    using System.Collections.Generic;
    using Common.Interfaces.Repositories;
    using Common.Interfaces.Services;
    using Common.Models;
    using FluentValidation;

    public class StoreService : IStoreService
    {
        #region Constructors

        public StoreService(IStoreRepository storeRepository)
        {
            this._storeRepository = storeRepository;

            this._storeValidator = new StoreValidator();
        }

        #endregion

        #region Private Properties

        private IStoreRepository _storeRepository { get; }

        private StoreValidator _storeValidator { get; }

        #endregion

        #region Public Methods

        public List<Store> Get()
        {
            var stores = this._storeRepository.Get();

            return stores;
        }

        public List<Store> StoreSearch(string storeName)
        {
            var stores = this._storeRepository.StoreSearch(storeName);

            return stores;
        }

        public List<Store> GetStoresByProductId(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException(nameof(productId));
            }

            var stores = this._storeRepository.GetStoresByProductId(productId);

            return stores;
        }

        public Store GetSingle(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            var store = this._storeRepository.GetSingle(storeId);

            return store;
        }

        public ServiceResult<Store> Insert(Store store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            var serviceResult = this._storeValidator.ValidateStore(store);

            if (!serviceResult.IsValid)
            {
                return serviceResult;
            }

            serviceResult.IsSuccessful = this._storeRepository.Insert(store);

            return serviceResult;
        }

        public ServiceResult<Store> Update(Store store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            var serviceResult = this._storeValidator.ValidateStore(store);

            if (!serviceResult.IsValid)
            {
                return serviceResult;
            }

            serviceResult.IsSuccessful = this._storeRepository.Update(store);

            return serviceResult;
        }

        public ServiceResult<Store> Delete(Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(storeId));
            }

            var serviceResult = new ServiceResult<Store>();

            serviceResult.IsSuccessful = this._storeRepository.Delete(storeId);

            return serviceResult;
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
