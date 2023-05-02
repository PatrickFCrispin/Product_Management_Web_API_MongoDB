using ProductManagement.API.DTOs;
using ProductManagement.API.Responses;
using ProductManagement.API.ViewModels;

namespace ProductManagement.API.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<ProductViewModel>> GetProductByIdAsync(string id);
        Task<ServiceCollectionResponse<ProductViewModel>> GetProductsAsync();
        Task<GenericResponse> AddProductAsync(ProductDTO productDTO);
        Task<GenericResponse> UpdateProductAsync(string id, ProductDTO productDTO);
        Task<GenericResponse> RemoveProductByIdAsync(string id);
    }
}