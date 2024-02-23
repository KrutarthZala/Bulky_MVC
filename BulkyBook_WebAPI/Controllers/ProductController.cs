using BulkyBook_WebAPI.Implementation;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #region Product GET
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                //var products = _unitOfWork.Product.GetAll();
                //var products = await _productService.GetProducts();
                var products = await _productService.GetProducts();

                // Validate categories
                if (products == null || !products.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No products found"
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }
        #endregion

        #region Product GET with Specific ProductID
        [HttpGet("{ProductID}")]
        public async Task<IActionResult> GetSpecificProduct(int ProductID)
        {
            // Validate ProductID
            if (ProductID < 1)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            //var Product = _unitOfWork.Product.Get(p => p.ProductID == ProductID);
            var Product = await _productService.GetProduct(ProductID);

            // Validate Product
            if (Product == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }
            return Ok(new { StatusCode = 200, Message = "Success", Data = Product });
        }
        #endregion 

        #region Product POST / Add Product
        [HttpPost]
        public async Task<IActionResult> PostProduct(IFormFile image, string productDetailsJson)
        {
            try
            {
                // Validate Product
                if (productDetailsJson == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Bad Request"
                    });
                }

                //await _unitOfWork.Product.Add(Product);
                //await _unitOfWork.Save();

                await _productService.InsertProduct(image, productDetailsJson);
                await _productService.SaveProduct();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product inserted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(406, new { Message = "Not Acceptable", Details = ex.Message });
            }
        }
        #endregion

        #region Product PUT / Update Product
        [HttpPut]
        public async Task<IActionResult> PutProduct(/*ProductModel product*/IFormFile image, string productDetailsJson, int productId)
        {
            // Validate ProductID and Product
            if (productDetailsJson == null || productId == 0)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            //newProduct.ProductName = Product.ProductName;
            //newProduct.ProductOrder = Product.ProductOrder;

            // await _unitOfWork.Save();
            await _productService.UpdateProduct(/*product*/image, productDetailsJson, productId);
            await _productService.SaveProduct();
            return Ok(new { StatusCode = 200, Message = "Product Updated successfully" });
        }
        #endregion

        #region Product DELETE
        [HttpDelete("{ProductID}")]
        public async Task<IActionResult> DeleteProduct(int ProductID)
        {
            // Validate ProductID
            if (ProductID < 1)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            var product = await _productService.GetProduct(ProductID);

            // Validate Product
            if (product == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }
            //await _unitOfWork.Product.Remove(Product);
            //await _unitOfWork.Save();
            await _productService.DeleteProduct(product);
            await _productService.SaveProduct();
            return Ok(new { StatusCode = 200, Message = "Product deleted successfully" });
        }
        #endregion

    }
}
