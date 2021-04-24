using MongoDB.Driver;
using MyLife.Models;

namespace MyLife.Data
{
    public class ApplicationContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Desire> Desires => _database.GetCollection<Desire>("Desires");
        public IMongoCollection<Habit> Habits => _database.GetCollection<Habit>("Habits");
        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public ApplicationContext()
        {
            IMongoClient Client = new MongoClient("mongodb+srv://dok1959:агслммыг2021@mylifecluster.yshbj.mongodb.net/mylifedb?retryWrites=true&w=majority");
            _database = Client.GetDatabase("mylifedb");
        }
    }
}
