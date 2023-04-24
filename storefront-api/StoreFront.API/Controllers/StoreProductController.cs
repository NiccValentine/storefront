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

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var storeProducts = this._storeProductService.Get();

                return this.StatusCode(200, storeProducts);
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

        [HttpGet("{storeId}/products/{productId}")]
        public ActionResult GetSingle(Guid storeId, Guid productId)
        {
            try
            {
                var storeProduct = this._storeProductService.GetSingle(storeId, productId);

                if (storeId == null)
                {
                    return this.StatusCode(204);
                }
                else if (productId == null)
                {
                    return this.StatusCode(204);
                }
                
                return this.StatusCode(200, storeProduct);
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

        [HttpPost("/storeproducts")]
        public ActionResult Post([FromBody] StoreProduct storeProduct)
        {
            try
            {

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
