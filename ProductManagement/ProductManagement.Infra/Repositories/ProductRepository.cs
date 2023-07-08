using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IOptions<MongoDBSettings> _options;
        private IMongoCollection<ProductEntity> _mongoCollection;

        public ProductRepository(IOptions<MongoDBSettings> options)
        {
            _options = options;
        }

        public IMongoCollection<ProductEntity> MongoCollection
        {
            get
            {
                if (_mongoCollection is null)
                {
                    var mongoClient = new MongoClient(_options.Value.ConnectionString);
                    var mongoDatabase = mongoClient.GetDatabase(_options.Value.DatabaseName);
                    _mongoCollection = mongoDatabase.GetCollection<ProductEntity>(_options.Value.CollectionName);
                }

                return _mongoCollection;
            }
        }

        public async Task<ProductEntity?> GetProductByIdAsync(string id)
        {
            try
            {
                return await MongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
        {
            try
            {
                return await MongoCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task AddProductAsync(ProductEntity productEntity)
        {
            try
            {
                productEntity.Active = true;
                productEntity.RegisteredAt = productEntity.ModifiedAt = DateTime.Now;

                await MongoCollection.InsertOneAsync(productEntity);
            }
            catch (Exception) { throw; }
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

                await MongoCollection.ReplaceOneAsync(x => x.Id == id, productToBeUpdated);

                return true;
            }
            catch (Exception) { throw; }
        }

        public async Task<bool> RemoveProductByIdAsync(string id)
        {
            try
            {
                if (await GetProductByIdAsync(id) is null) { return false; }

                await MongoCollection.DeleteOneAsync(x => x.Id == id);

                return true;
            }
            catch (Exception) { throw; }
        }
    }
}