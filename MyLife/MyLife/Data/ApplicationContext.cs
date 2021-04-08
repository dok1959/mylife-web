using MongoDB.Driver;

namespace MyLife.Data
{
    public class ApplicationContext
    {
        public IMongoClient Client;
        public IMongoDatabase Database;
        public ApplicationContext()
        {
            Client = new MongoClient("mongodb+srv://dok1959:агслммыг2021@mylifecluster.yshbj.mongodb.net/mylifedb?retryWrites=true&w=majority");
            Database = Client.GetDatabase("mylifedb");
        }
    }
}
