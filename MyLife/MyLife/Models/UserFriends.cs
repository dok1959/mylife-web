using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MyLife.Models
{
    public class UserFriends
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Available { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Sent { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Received { get; set; }

        public UserFriends()
        {
            Available = new List<string>();
            Sent = new List<string>();
            Received = new List<string>();
        }
    }
}
