using MongoDB.Bson.Serialization.Attributes;

namespace Constracts.Domain
{
    public abstract class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("_id")]
        public virtual string Id { get; init; }
        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }
        [BsonElement("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; } 


    }
}
