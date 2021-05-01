using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyLife.ViewModels;

namespace MyLife.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

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

        [BsonRequired]
        [BsonElement("hashedPassword")]
        public string HashedPassword { get; set; }

        public UserFriends Friends { get; set; }

        public string RefreshToken { get; set; }

        public User()
        {
            Friends = new UserFriends();
        }

        public User(RegisterViewModel model)
        {
            Login = model.Login;
            Username = model.Username;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
            Friends = new UserFriends();
        }
    }
}