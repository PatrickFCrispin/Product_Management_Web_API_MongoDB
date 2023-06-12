using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductManagement.Domain.Entities
{
    public class ProductEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Supplier { get; set; } = default!;
        public bool Active { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}