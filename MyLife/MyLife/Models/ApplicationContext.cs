using MongoDB.Bson;
using MongoDB.Driver;

namespace MyLife.Models
{
    public class ApplicationContext
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        public ApplicationContext()
        {
            _client = new MongoClient("mongodb+srv://dok1959:агслммыг2021@mylifecluster.yshbj.mongodb.net/mylifedb?retryWrites=true&w=majority");

            _database = _client.GetDatabase("mylifedb");

            /*var collection = database.GetCollection<BsonDocument>("mylifecollection");
            BsonDocument person1 = new BsonDocument
            {
                {"Name", "Bill"},
                {"Age", 32},
                {"Languages", new BsonArray{"english", "german"}}
            };
            collection.InsertOne(person1);*/
        }
    }
}
