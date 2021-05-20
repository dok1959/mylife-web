using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyLife.Models.TargetModels
{
    public class SubProgress
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Title { get; set; }
        public ProgressValue Value { get; set; }
        public int Order { get; set; }
    }
}
