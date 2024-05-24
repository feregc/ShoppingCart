using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Services;
using ShoppingCartApi.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace ShoppingCartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        protected ResponseDto _response;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
            _response = new ResponseDto();
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetFakeStoreProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();
                _response.Result = products;
                _response.DisplayMessage = "Products List";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return Ok(_response);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                _response.Result = product;
                _response.DisplayMessage = "Product Information";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
            return Ok(_response);
        }

        //[HttpPost("product")]
        //public async Task<IActionResult> AddProduct(int id)
        //{
        //    try
        //    {
        //        //var productData = await _productService.GetProductByIdAsync(id);
        //        //var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(productData);
        //        //product.rating = null;

        //        //var addedProduct = await _productService.AddProductAsync(product);
        //        //_response.Result = addedProduct;
        //        //_response.DisplayMessage = "Product added successfully";

        //        var productData = await _productService.GetProductByIdAsync(id);
        //        var productJson = JObject.Parse(productData);

        //        // Eliminar el campo 'rating' del JSON
        //        productJson.Remove("rating");

        //        var product = productJson.ToObject<Product>();

        //        var addedProduct = await _productService.AddProductAsync(product);
        //        _response.Result = addedProduct;
        //        _response.DisplayMessage = "Product added successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string> { ex.Message };
        //        return StatusCode(500, _response);
        //    }
        //    return Ok(_response);
        //}

        [HttpPost("product")]
        public async Task<IActionResult> AddProduct(int id)
        {
            try
            {
                var productData = await _productService.GetProductByIdAsync(id);
                var productJson = JObject.Parse(productData);

                // Eliminar el campo 'rating' del JSON
                productJson.Remove("rating");

                var product = productJson.ToObject<Product>();

                var addedProduct = await _productService.AddProductAsync(product);
                _response.Result = addedProduct;
                _response.DisplayMessage = "Product added successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> {
            "An error occurred while saving the entity changes. See the inner exception for details.",
            ex.InnerException?.Message // Agregamos el mensaje de la excepción interna
        };
                return StatusCode(500, _response);
            }
            return Ok(_response);
        }
    }
}
