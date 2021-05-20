using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MyLife.Models.TargetModels
{
    public class Progress
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; }
        public DateTime Date { get; set; }
        public ProgressValue Value { get; set; }
        public List<SubProgress> CheckBocks { get; set; }
    }
}
