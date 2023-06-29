namespace StoreFront.API.Controllers
{
    using System;
    using Common.Interfaces.Services;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class StoreController : ControllerBase
    {
        public StoreController(IStoreService storeService)
        {
            this._storeService = storeService;
        }

        private IStoreService _storeService { get; }

        [HttpGet("/stores")]
        public ActionResult Get()
        {
            try
            {
                var stores = this._storeService.Get();

                if (stores.Count == 0)
                {
                    return this.StatusCode(204);
                }
                else
                {
                    return this.StatusCode(200, stores);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                return this.StatusCode(500);
            }
        }

        

        [HttpGet("/product/{productId}/stores")]
        public ActionResult GetStoresByProductId(Guid productId)
        {
            try
            {
                if (productId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(productId));
                }

                var products = this._storeService.GetStoresByProductId(productId);

                return this.StatusCode(200, products);
            }
            catch (ArgumentNullException argumentNullException)
            {

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                return this.StatusCode(500);
            }
        }

        [HttpGet("/stores/{storeId}")]
        public ActionResult GetSingle(Guid storeId)
        {
            try
            {
                if (storeId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(storeId));
                }

                var store = this._storeService.GetSingle(storeId);

                if (store == null)
                {
                    return this.StatusCode(204);
                }

                return this.StatusCode(200, store);
            }
            catch (ArgumentNullException argumentNullException)
            {
                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                return this.StatusCode(500);
            }
        }

        [HttpPost("/stores")]
        public ActionResult Post([FromBody]Store store)
        {
            try
            {
                if (store == null)
                {
                    throw new ArgumentNullException(nameof(store));
                }

                var serviceResult = this._storeService.Insert(store);
                
                if (serviceResult.IsSuccessful)
                {
                    return this.StatusCode(201, serviceResult);
                }
                else
                {
                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                return this.StatusCode(500);
            }
        }

        [HttpPut("/stores/{storeId}")]
        public ActionResult Put([FromBody]Store store)
        {
            try
            {
                if (store == null)
                {
                    throw new ArgumentNullException(nameof(store));
                }

                var serviceResult = this._storeService.Update(store);

                if (serviceResult.IsSuccessful)
                {
                    return this.StatusCode(200, serviceResult);
                }
                else
                {
                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                return this.StatusCode(500);
            }
        }

        [HttpDelete("/stores/{storeId}")]
        public ActionResult Delete(Guid storeId)
        {
            try
            {
                if (storeId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(storeId));
                }

                var serviceResult = this._storeService.Delete(storeId);

                if (serviceResult.IsSuccessful == true)
                {
                    return this.StatusCode(200, serviceResult);
                }
                else
                {
                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                return this.StatusCode(500);
            }
        }

        [HttpGet("/stores/search")]
        public ActionResult StoreSearch([FromQuery] string storeName)
        {
            try
            {
                var stores = this._storeService.StoreSearch(storeName);

                if (stores.Count == 0)
                {
                    return this.StatusCode(204);
                }

                return this.StatusCode(200, stores);
            }
            catch (ArgumentNullException argumentNullException)
            {
                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                return this.StatusCode(500);
            }
        }
    }
}
