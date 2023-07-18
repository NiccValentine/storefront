namespace StoreFront.API.Controllers
{
    using System;
    using Common.Interfaces.Services;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;
    using StoreFront.Common.Interfaces.Logging;

    [ApiController]
    public class StoreController : ControllerBase
    {
        #region Constructors
        public StoreController(IStoreService storeService, ILogService logService)
        {
            this._storeService = storeService;

            this._logService = logService;
        }
        #endregion

        #region Private Properties
        private IStoreService _storeService { get; }

        private ILogService _logService { get; }
        #endregion

        #region Public Methods

        [HttpGet("/stores")]
        public ActionResult Get()
        {
            try
            {
                this._logService.Debug("StoreController.Get called");

                var stores = this._storeService.Get();

                if (stores.Count == 0)
                {
                    this._logService.Trace("StoreController.Get returned 0 results");

                    return this.StatusCode(204);
                }
                else
                {
                    this._logService.Trace("StoreController.Get returned {0} result(s)", stores.Count);

                    return this.StatusCode(200, stores);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreController.Get exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreController.Get exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreController.Get exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        

        [HttpGet("/product/{productId}/stores")]
        public ActionResult GetStoresByProductId(Guid productId)
        {
            try
            {
                this._logService.Debug("StoreController.GetStoresByProductId called");

                if (productId == Guid.Empty)
                {
                    this._logService.Warn("StoreController.GetStoresByProductId productId is not present");

                    throw new ArgumentException(nameof(productId));
                }

                var stores = this._storeService.GetStoresByProductId(productId);

                this._logService.Trace("StoreController.GetStoresByProductId has returned {0} result(s)", stores.Count);

                return this.StatusCode(200, stores);
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreController.GetStoresByProductId exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreController.GetStoresByProductId exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreController.GetStoresByProductId exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpGet("/stores/{storeId}")]
        public ActionResult GetSingle(Guid storeId)
        {
            try
            {
                this._logService.Debug("StoreController.GetSingle called");

                if (storeId == Guid.Empty)
                {
                    this._logService.Warn("StoreController.GetSingle storeId is not present");

                    throw new ArgumentException(nameof(storeId));
                }

                var store = this._storeService.GetSingle(storeId);

                if (store == null)
                {
                    this._logService.Trace("StoreController.GetSingle has returned 0 results");

                    return this.StatusCode(204);
                }

                this._logService.Trace("StoreController.GetSingle has returned 1 result");

                return this.StatusCode(200, store);
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreController.GetSingle exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreController.GetSingle exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreController.GetSingle exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpPost("/stores")]
        public ActionResult Post([FromBody]Store store)
        {
            try
            {
                this._logService.Debug("StoreController.Post called");

                if (store == null)
                {
                    this._logService.Warn("StoreController.Post store is null");

                    throw new ArgumentNullException(nameof(store));
                }

                var serviceResult = this._storeService.Insert(store);
                
                if (serviceResult.IsSuccessful)
                {
                    this._logService.Trace("StoreController.Post has inserted data");

                    return this.StatusCode(201, serviceResult);
                }
                else
                {
                    this._logService.Warn("StoreController.Post has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreController.Post exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreController.Post exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreController.Post exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpPut("/stores/{storeId}")]
        public ActionResult Put([FromBody]Store store)
        {
            try
            {
                this._logService.Debug("StoreController.Put called");

                if (store == null)
                {
                    this._logService.Warn("StoreController.Put store is null");

                    throw new ArgumentNullException(nameof(store));
                }

                var serviceResult = this._storeService.Update(store);

                if (serviceResult.IsSuccessful)
                {
                    this._logService.Trace("StoreController.Put has altered the database");

                    return this.StatusCode(200, serviceResult);
                }
                else
                {
                    this._logService.Warn("StoreController.Put has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreController.Put exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreController.Put exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreController.Put exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpDelete("/stores/{storeId}")]
        public ActionResult Delete(Guid storeId)
        {
            try
            {
                this._logService.Debug("StoreController.Delete called");

                if (storeId == Guid.Empty)
                {
                    this._logService.Warn("StoreController.Delete storeId is not present");

                    throw new ArgumentException(nameof(storeId));
                }

                var serviceResult = this._storeService.Delete(storeId);

                if (serviceResult.IsSuccessful == true)
                {
                    this._logService.Trace("StoreController.Delete has successfully removed data");

                    return this.StatusCode(200, serviceResult);
                }
                else
                {
                    this._logService.Warn("StoreController.Delete has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreController.Delete exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreController.Delete exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreController.Delete exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpGet("/stores/search")]
        public ActionResult StoreSearch([FromQuery] string storeName)
        {
            try
            {
                this._logService.Debug("StoreController.StoreSearch called");

                var stores = this._storeService.StoreSearch(storeName);

                if (stores.Count == 0)
                {
                    this._logService.Trace("StoreController.StoreSearch has returned 0 results");

                    return this.StatusCode(204);
                }

                this._logService.Trace("StoreController.StoreSearch has returned {0} result(s)", stores);

                return this.StatusCode(200, stores);
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreController.StoreSearch exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreController.StoreSearch exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreController.StoreSearch exception: {0}", exception);

                return this.StatusCode(500);
            }
        }
        #endregion
    }
}
