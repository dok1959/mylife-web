using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyLife.Models
{
    public class TargetInvitation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string TargetId { get; set; }

        public TargetInvitation(string senderId, string targetId)
        {
            SenderId = senderId;
            TargetId = targetId;
        }
    }
}
