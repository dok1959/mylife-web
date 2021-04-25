using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyLife.ViewModels;

namespace MyLife.Models
{
    public class SubDesire
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Title { get; set; }
        public float MaxValue { get; set; }
        public float CurrentValue { get; set; }
        public int Order { get; set; }

        public SubDesire(SubDesireViewModel model)
        {
            Title = model.Title;
            MaxValue = model.MaxValue;
            CurrentValue = model.CurrentValue;
            Order = model.Order;
        }
    }
}
