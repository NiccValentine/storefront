namespace StoreFront.API.Controllers
{
    using System;
    using Common.Interfaces.Services;
    using Common.Models;
    using Microsoft.AspNetCore.Mvc;
    using StoreFront.Common.Interfaces.Logging;

    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Constructors
        public ProductController(IProductService productService, ILogService logService)
        {
            this._productService = productService;

            this._logService = logService;
        }
        #endregion

        #region Private Properties
        private IProductService _productService { get; }
        private ILogService _logService { get; }
        #endregion

        #region Public Methods

        [HttpGet("/products")]
        public ActionResult Get()
        {
            try
            {
                this._logService.Debug("ProductController.Get called.");

                var products = this._productService.Get();

                if (products.Count == 0)
                {
                    this._logService.Trace("ProductController.Get returned 0 results");

                    return this.StatusCode(204);
                }
                else
                {
                    this._logService.Trace("ProductController.Get returned {0} result(s)", products.Count);

                    return this.StatusCode(200, products);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.Get exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.Get exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.Get exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpGet("/stores/{storeId}/products/not")]
        public ActionResult GetProductsNotMatchingStoreId(Guid storeId)
        {
            try
            {
                this._logService.Debug("ProductController.GetProductsNotMatchingStoreId called");

                if (storeId == Guid.Empty)
                {
                    this._logService.Warn("ProductController.Get storeId is not present");

                    throw new ArgumentException(nameof(storeId));
                }

                var stores = this._productService.GetProductsNotMatchingStoreId(storeId);

                this._logService.Trace("ProductController.GetProductsNotMatchingStoreId returned {0} result(s)", stores.Count);

                return this.StatusCode(200, stores);
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.GetProductsNotMatchingStoreId exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.GetProductsNotMatchingStoreId exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.GetProductsNotMatchingStoreId exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpGet("stores/{storeId}/products")]
        public ActionResult GetProductsByStoreId(Guid storeId)
        {
            try
            {
                this._logService.Debug("ProductController.GetProductsByStoreId called");

                if (storeId == Guid.Empty)
                {
                    this._logService.Warn("ProductController.GetProductsByStoreId storeId is not present");

                    throw new ArgumentException(nameof(storeId));
                }

                var products = this._productService.GetProductsByStoreId(storeId);

                this._logService.Trace("ProductController.GetProductsByStoreId returned {0} result(s)", products.Count);

                return this.StatusCode(200, products);
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.GetProductsByStoreId exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.GetProductsByStoreId exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.GetProductsByStoreId exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpGet("/products/{productId}")]
        public ActionResult GetSingle(Guid productId)
        {
            try
            {
                this._logService.Debug("ProductController.GetSingle called");

                if (productId == Guid.Empty)
                {
                    this._logService.Warn("ProductController.GetSingle productId is not present");

                    throw new ArgumentException(nameof(productId));
                }

                var product = this._productService.GetSingle(productId);
                
                if (product == null)
                {
                    this._logService.Trace("ProductController.GetSingle product returned no result");

                    return this.StatusCode(204);
                }

                this._logService.Trace("ProductController.GetSingle returned 1 result");

                return this.StatusCode(200, product);
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.GetSingle exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.GetSingle exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.GetSingle exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpPost("/products")]
        public ActionResult Post([FromBody]Product product)
        {
            try
            {
                this._logService.Debug("ProductController.Post called");

                if (product == null)
                {
                    this._logService.Warn("ProductController.Post product is null");

                    throw new ArgumentNullException(nameof(product));
                }

                var serviceResult = this._productService.Insert(product);

                if (serviceResult.IsSuccessful)
                {
                    this._logService.Trace("ProductController.Post has inserted data");

                    return this.StatusCode(201, serviceResult);
                }
                else
                {
                    this._logService.Warn("ProductController.Post has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.Post exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.Post exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.Post exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpPut("/products/{productId}")]
        public ActionResult Put([FromBody]Product product)
        {
            try
            {
                this._logService.Debug("ProductController.Put called");

                if (product == null)
                {
                    this._logService.Warn("ProductController.Put product is null");

                    throw new ArgumentNullException(nameof(product));
                }

                var serviceResult = this._productService.Update(product);

                if (serviceResult.IsSuccessful)
                {
                    this._logService.Trace("ProductController.Put has altered the database");

                    return this.StatusCode(200, serviceResult);
                }
                else
                {
                    this._logService.Warn("ProductController.Put has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.Put exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.Put exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.Put exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpDelete("/products/{productId}")]
        public ActionResult Delete(Guid productId)
        {
            try
            {
                this._logService.Debug("ProductController.Delete called");

                if (productId == Guid.Empty)
                {
                    this._logService.Warn("ProductController.Delete productId is not present");

                    throw new ArgumentException();
                }

                var serviceResult = this._productService.Delete(productId);

                if (serviceResult.IsSuccessful == true)
                {
                    this._logService.Trace("ProductController.Delete has successfully removed data");

                    return this.StatusCode(200, serviceResult);
                }
                else
                {
                    this._logService.Warn("ProductController.Delete has encountered a validation error");

                    return this.StatusCode(400, serviceResult);
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.Delete exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.Delete exception: {0}", argumentException);

                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.Delete exception: {0}", exception);

                return this.StatusCode(500);
            }
        }

        [HttpGet("/products/search")]
        public ActionResult ProductSearch([FromQuery] string productName)
        {
            try
            {
                this._logService.Debug("ProductController.ProductSearch called");

                var products = this._productService.ProductSearch(productName);

                if (products.Count == 0)
                {
                    this._logService.Trace("ProductController.ProductSearch returned 0 results");

                    return this.StatusCode(204);
                }

                this._logService.Trace("ProductController.ProductSearch returned {0} result(s)", products.Count);

                return this.StatusCode(200, products);
            }
            catch (ArgumentNullException argumentNullException)
            {
                this._logService.Error("ProductController.ProductSearch exception: {0}", argumentNullException);

                return this.StatusCode(400);
            }
            catch (ArgumentException argumentException)
            {
                this._logService.Error("ProductController.ProductSearch exception: {0}", argumentException);


                return this.StatusCode(400);
            }
            catch (Exception exception)
            {
                this._logService.Error("ProductController.ProductSearch exception: {0}", exception);


                return this.StatusCode(500);
            }
        }
        #endregion
    }
}
