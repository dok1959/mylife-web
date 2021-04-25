using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyLife.ViewModels;
using System;
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

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public bool IsPrivate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Members { get; set; }

        public List<SubDesire> SubDesires { get; set; }

        public Desire(DesireViewModel model)
        {
            Title = model.Title;
            Description = model.Description;
            Type = model.Type;
            Date = model.Date;
            IsPrivate = model.IsPrivate;
            Members = new List<string>();
        }
    }
}
