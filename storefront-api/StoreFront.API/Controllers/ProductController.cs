namespace StoreFront.API.Controllers
{
    using System;
    using Common.Interfaces.Services;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class ProductController : ControllerBase
    {
        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        private IProductService _productService { get; }

        [HttpGet("/products")]
        public ActionResult Get()
        {
            try
            {
                var products = this._productService.Get();

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

        [HttpGet("/stores/{storeId}/products/not")]
        public ActionResult GetProductsNotMatchingStoreId(Guid storeId)
        {
            try
            {
                if (storeId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(storeId));
                }

                var stores = this._productService.GetProductsNotMatchingStoreId(storeId);

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

        [HttpGet("/products/search")]
        public ActionResult ProductSearch([FromQuery]string productName)
        {
            try
            {
                var products = this._productService.ProductSearch(productName);

                if (products.Count == 0)
                {
                    return this.StatusCode(204);
                }

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

        [HttpGet("stores/{storeId}/products")]
        public ActionResult GetProductsByStoreId(Guid storeId)
        {
            try
            {
                if (storeId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(storeId));
                }

                var products = this._productService.GetProductsByStoreId(storeId);

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

        [HttpGet("/products/{productId}")]
        public ActionResult GetSingle(Guid productId)
        {
            try
            {
                if (productId == Guid.Empty)
                {
                    throw new ArgumentException(nameof(productId));
                }

                var product = this._productService.GetSingle(productId);
                
                if (product == null)
                {
                    return this.StatusCode(204);
                }

                return this.StatusCode(200, product);
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

        [HttpPost("/products")]
        public ActionResult Post([FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                var serviceResult = this._productService.Insert(product);

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

        [HttpPut("/products/{productId}")]
        public ActionResult Put([FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                var serviceResult = this._productService.Update(product);

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

        [HttpDelete("/products/{productId}")]
        public ActionResult Delete(Guid productId)
        {
            try
            {

                if (productId == Guid.Empty)
                {
                    throw new ArgumentException();
                }

                var serviceResult = this._productService.Delete(productId);

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
