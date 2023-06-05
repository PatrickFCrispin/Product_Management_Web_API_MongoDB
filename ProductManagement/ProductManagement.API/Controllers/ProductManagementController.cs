using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.DTOs;
using ProductManagement.API.Responses;
using ProductManagement.API.Services;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductManagementController : ControllerBase
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
            if (response.Success)
            {
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

            _logger.LogError("GetProductByIdAsync::Error -> {error}", response.Message);
            return BadRequest(response);
        }

        [HttpGet] // api/products
        public async Task<ActionResult> GetProductsAsync()
        {
            var response = await _productService.GetProductsAsync();
            if (response.Success)
            {
                _logger.LogInformation("GetProductsAsync::Info -> Quantidade de produtos cadastrados {count}", response.Data?.ToList().Count);
                return Ok(response);
            }

            _logger.LogError("GetProductsAsync::Error -> {error}", response.Message);
            return BadRequest(response);
        }

        [HttpPost] // api/products
        //If there was views, we can use view models instead of dto. Same for UpdateProductAsync.
        public async Task<ActionResult> AddProductAsync([FromBody] ProductDTO productDTO)
        {
            var validator = new ProductDTOValidator();
            var validationResult = validator.Validate(productDTO);
            if (validationResult.IsValid)
            {
                var response = await _productService.AddProductAsync(productDTO);
                if (response.Success)
                {
                    _logger.LogInformation("AddProductAsync::Info -> {message}", response.Message);
                    return Ok(response);
                }

                _logger.LogError("AddProductAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            var errors = ErrorResponse.ToErrorResponse(validationResult.Errors);
            return BadRequest(errors);
        }

        [HttpPut("{id:length(24)}")] // api/products/id
        public async Task<ActionResult> UpdateProductAsync(string id, [FromBody] ProductDTO productDTO)
        {
            var validator = new ProductDTOValidator();
            var validationResult = validator.Validate(productDTO);
            if (validationResult.IsValid)
            {
                var response = await _productService.UpdateProductAsync(id, productDTO);
                if (response.Success)
                {
                    _logger.LogInformation("UpdateProductAsync::Info -> {message}", response.Message);
                    return Ok(response);
                }

                _logger.LogError("UpdateProductAsync::Error -> {error}", response.Message);
                return BadRequest(response);
            }

            var errors = ErrorResponse.ToErrorResponse(validationResult.Errors);
            return BadRequest(errors);
        }

        [HttpDelete("{id:length(24)}")] // api/products/id
        public async Task<ActionResult> RemoveProductByIdAsync(string id)
        {
            var response = await _productService.RemoveProductByIdAsync(id);
            if (response.Success)
            {
                _logger.LogInformation("RemoveProductByIdAsync::Info -> {message}", response.Message);
                return Ok(response);
            }

            _logger.LogError("RemoveProductByIdAsync::Error -> {error}", response.Message);
            return BadRequest(response);
        }
    }
}