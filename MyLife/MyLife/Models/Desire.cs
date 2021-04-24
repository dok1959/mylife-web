using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyLife.ViewModels;
using System.Collections.Generic;

namespace MyLife.Models
{
    public class Desire
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> UsersId { get; set; }

        public Desire(DesireViewModel model)
        {
            Title = model.Title;
            Description = model.Description;
            UsersId = model.UsersId;
        }
    }
}
