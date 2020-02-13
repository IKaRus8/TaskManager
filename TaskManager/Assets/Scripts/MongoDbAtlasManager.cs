using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Linq;

namespace DataBase
{
    class MongoDbAtlasManager
    {
        public MongoClient _client;
        public IMongoDatabase _database;
        public IMongoCollection<BsonDocument> UserCollection;

        private User _user;

        private void Conect()
        {
            _client = new MongoClient("mongodb+srv://Rkarimov:12345@clustertask-nfnnn.gcp.mongodb.net/test?retryWrites=true&w=majority");
            _database = _client.GetDatabase("test");
            UserCollection = _database.GetCollection<BsonDocument>("Users");
        }

        public void TakeUser(string login, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("login", login);

            var response = UserCollection.Find(filter).FirstOrDefault();

            var a = JsonConvert.DeserializeObject(response.ToString());
        }

        public string NewUser(string login, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("login", login);

            var checkUser = UserCollection.Find(filter).FirstOrDefault();

            if(checkUser != null)
            {
                return "User already created";
            }

            _user = new User
            {
                login = login,
                password = password
            };

            var document = new BsonDocument { { "login", login }, {"password", password}};

            UserCollection.InsertOne(document);

            checkUser = UserCollection.Find(filter).FirstOrDefault();

            return checkUser.ToString();
        }
    }
}