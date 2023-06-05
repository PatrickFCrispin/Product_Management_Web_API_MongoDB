using ProductManagement.API.DTOs;
using ProductManagement.API.Responses;

namespace ProductManagement.API.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(string id);
        Task<ServiceCollectionResponse<ProductDTO>> GetProductsAsync();
        Task<GenericResponse> AddProductAsync(ProductDTO productDTO);
        Task<GenericResponse> UpdateProductAsync(string id, ProductDTO productDTO);
        Task<GenericResponse> RemoveProductByIdAsync(string id);
    }
}