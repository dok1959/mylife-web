﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MyLife.Models
{
    public class User
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }

        [BsonRequired]
        [BsonElement("login")]
        public string Login { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [JsonIgnore]
        [BsonRequired]
        [BsonElement("password")]
        public string Password { get; set; }
    }
}
