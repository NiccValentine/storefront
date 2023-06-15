namespace StoreFront.API.Controllers
{
    using System;
    using Common.Interfaces.Services;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("storeproducts")]
    public class StoreProductController : ControllerBase
    {
        public StoreProductController(IStoreProductService storeProductService)
        {
            this._storeProductService = storeProductService;
        }

        private IStoreProductService _storeProductService { get; }

        [HttpPost("/storeproducts")]
        public ActionResult Post([FromBody] StoreProduct storeProduct)
        {
            try
            {
                if (storeProduct == null)
                {
                    throw new ArgumentNullException(nameof(storeProduct));
                }

                var serviceResult = this._storeProductService.Insert(storeProduct);

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

        [HttpDelete("{storeId}/products/{productId}")]
        public ActionResult Delete(Guid storeId, Guid productId)
        {
            try
            {
                if (storeId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(storeId));
                }

                if (productId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(productId));
                }

                var serviceResult = this._storeProductService.Delete(storeId, productId);

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
    }
}
