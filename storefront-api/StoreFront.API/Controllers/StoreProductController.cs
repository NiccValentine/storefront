namespace StoreFront.API.Controllers
{
    using System;
    using Common.Interfaces.Services;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;
    using StoreFront.Common.Interfaces.Logging;

    [ApiController]
    [Route("storeproducts")]
    public class StoreProductController : ControllerBase
    {
        #region Constructors
        public StoreProductController(IStoreProductService storeProductService, ILogService logService)
        {
            this._storeProductService = storeProductService;

            this._logService = logService;
        }
        #endregion

        #region Private Properties
        private IStoreProductService _storeProductService { get; }

        private ILogService _logService { get; }
        #endregion

        #region Public Methods
        [HttpPost("/storeproducts")]
        public ActionResult Post([FromBody] StoreProduct storeProduct)
        {
            try
            {
                this._logService.Debug("StoreProductController.Post called");

                if (storeProduct == null)
                {
                    this._logService.Warn("StoreProductController.Post storeProduct is null");

                    throw new ArgumentNullException(nameof(storeProduct));
                }

                var serviceResult = this._storeProductService.Insert(storeProduct);

                if (serviceResult.IsSuccessful)
                {
                    this._logService.Trace("StoreProductController.Post has inserted data");

                    return this.StatusCode(201, serviceResult);
                }
                else
                {
                    this._logService.Warn("StoreProductController.Post has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreProductController.Post exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreProductController.Post exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreProductController.Post exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpDelete("{storeId}/products/{productId}")]
        public ActionResult Delete(Guid storeId, Guid productId)
        {
            try
            {
                this._logService.Debug("StoreProductController.Delete has been called");

                if (storeId == Guid.Empty)
                {
                    this._logService.Warn("StoreProductController.Delete storeId is not present");

                    throw new ArgumentException(nameof(storeId));
                }

                if (productId == Guid.Empty)
                {
                    this._logService.Warn("StoreProductController.Delete productId is not present");

                    throw new ArgumentException(nameof(productId));
                }

                var serviceResult = this._storeProductService.Delete(storeId, productId);

                if (serviceResult.IsSuccessful == true)
                {
                    this._logService.Trace("StoreProductController.Delete has successfully removed data");

                    return this.StatusCode(200, serviceResult);
                }
                else
                {
                    this._logService.Warn("StoreProductController.Delete has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("StoreProductController.Delete exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("StoreProductController.Delete exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("StoreProductController.Delete exception: {0}", exception);

                return this.StatusCode(500);
            }
        }
        #endregion
    }
}
