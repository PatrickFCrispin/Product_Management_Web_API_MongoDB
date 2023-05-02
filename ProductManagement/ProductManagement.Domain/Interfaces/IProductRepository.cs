using ProductManagement.Domain.Entities;

namespace ProductManagement.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity?> GetProductByIdAsync(string id);
        Task<IEnumerable<ProductEntity>> GetProductsAsync();
        Task AddProductAsync(ProductEntity productEntity);
        Task<bool> UpdateProductAsync(string id, ProductEntity productEntity);
        Task<bool> RemoveProductByIdAsync(string id);
    }
}