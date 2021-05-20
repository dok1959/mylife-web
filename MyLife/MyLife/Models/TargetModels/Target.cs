using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MyLife.Models.TargetModels
{
    public class Target
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Members { get; set; }

        public List<Progress> Progress { get; set; }

        public Target()
        {
            Progress = new List<Progress>();
        }
    }
}
