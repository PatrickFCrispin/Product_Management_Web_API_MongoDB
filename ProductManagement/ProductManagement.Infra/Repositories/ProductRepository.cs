using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<ProductEntity> _mongoCollection;

        public ProductRepository(IOptions<MongoDBSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
            _mongoCollection = mongoDatabase.GetCollection<ProductEntity>(options.Value.CollectionName);
        }

        public async Task<ProductEntity?> GetProductByIdAsync(string id)
        {
            try
            {
                return await _mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            }
            catch { throw; }
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
        {
            try
            {
                return await _mongoCollection.Find(_ => true).ToListAsync();
            }
            catch { throw; }
        }

        public async Task AddProductAsync(ProductEntity productEntity)
        {
            try
            {
                productEntity.Active = true;
                productEntity.RegisteredAt = productEntity.ModifiedAt = DateTime.Now;

                await _mongoCollection.InsertOneAsync(productEntity);
            }
            catch { throw; }
        }

        public async Task<bool> UpdateProductAsync(string id, ProductEntity productEntity)
        {
            try
            {
                var productToBeUpdated = await GetProductByIdAsync(id);
                if (productToBeUpdated is null) { return false; }

                productToBeUpdated.Name = productEntity.Name;
                productToBeUpdated.Price = productEntity.Price;
                productToBeUpdated.Supplier = productEntity.Supplier;
                productToBeUpdated.Active = productEntity.Active;
                productToBeUpdated.ModifiedAt = DateTime.Now;
                await _mongoCollection.ReplaceOneAsync(x => x.Id == id, productToBeUpdated);

                return true;
            }
            catch { throw; }
        }

        public async Task<bool> RemoveProductByIdAsync(string id)
        {
            try
            {
                if (await GetProductByIdAsync(id) is null) { return false; }

                await _mongoCollection.DeleteOneAsync(x => x.Id == id);

                return true;
            }
            catch { throw; }
        }
    }
}