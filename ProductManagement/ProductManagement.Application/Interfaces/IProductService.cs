using ProductManagement.Application.DTOs;
using ProductManagement.Application.Responses;

namespace ProductManagement.Application.Interfaces
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
