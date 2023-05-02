using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductManagement.Domain.Entities
{
    public class ProductEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }
        public string Supplier { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}