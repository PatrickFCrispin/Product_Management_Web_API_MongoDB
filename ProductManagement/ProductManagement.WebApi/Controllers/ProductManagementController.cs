using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Responses;

namespace ProductManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductManagementController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductManagementController> _logger;

        public ProductManagementController(IProductService productService, ILogger<ProductManagementController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("{id:length(24)}")] // api/products/id
        public async Task<ActionResult> GetProductByIdAsync(string id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Success)
            {
                _logger.LogError("GetProductByIdAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            if (response.Data is null)
            {
                _logger.LogInformation("GetProductByIdAsync::Info -> {message}", response.Message);
                return NotFound(response);
            }

            _logger.LogInformation(
                "GetProductByIdAsync::Info -> Id {id}, Nome {name}, Preço {price}, Fornecedor {supplier}, Ativo {active}, Cadastrado em {registeredAt}, Editado em {modifiedAt}",
                response.Data.Id,
                response.Data.Name,
                response.Data.Price,
                response.Data.Supplier,
                response.Data.Active,
                response.Data.RegisteredAt,
                response.Data.ModifiedAt);
            return Ok(response);
        }

        [HttpGet] // api/products
        public async Task<ActionResult> GetProductsAsync()
        {
            var response = await _productService.GetProductsAsync();
            if (!response.Success)
            {
                _logger.LogError("GetProductsAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("GetProductsAsync::Info -> Quantidade de produtos cadastrados {count}", response.Data?.ToList().Count);
            return Ok(response);
        }

        [HttpPost] // api/products
        //If there was views, we can use view models instead of dto. Same for UpdateProductAsync.
        public async Task<ActionResult> AddProductAsync([FromBody] ProductDTO productDTO)
        {
            var validator = new ProductDTOValidator();
            var validationResult = validator.Validate(productDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToErrorResponse();
                return BadRequest(errors);
            }

            var response = await _productService.AddProductAsync(productDTO);
            if (!response.Success)
            {
                _logger.LogError("AddProductAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("AddProductAsync::Info -> {message}", response.Message);
            return Ok(response);
        }

        [HttpPut("{id:length(24)}")] // api/products/id
        public async Task<ActionResult> UpdateProductAsync(string id, [FromBody] ProductDTO productDTO)
        {
            var validator = new ProductDTOValidator();
            var validationResult = validator.Validate(productDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToErrorResponse();
                return BadRequest(errors);
            }

            var response = await _productService.UpdateProductAsync(id, productDTO);
            if (!response.Success)
            {
                _logger.LogError("UpdateProductAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("UpdateProductAsync::Info -> {message}", response.Message);
            return Ok(response);
        }

        [HttpDelete("{id:length(24)}")] // api/products/id
        public async Task<ActionResult> RemoveProductByIdAsync(string id)
        {
            var response = await _productService.RemoveProductByIdAsync(id);
            if (!response.Success)
            {
                _logger.LogError("RemoveProductByIdAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("RemoveProductByIdAsync::Info -> {message}", response.Message);
            return Ok(response);
        }
    }
}
