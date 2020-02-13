using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

        public void Conect()
        {
            _client = new MongoClient("mongodb+srv://Rkarimov:12345@clustertask-nfnnn.gcp.mongodb.net/test?retryWrites=true&w=majority");
            _database = _client.GetDatabase("test");
            UserCollection = _database.GetCollection<BsonDocument>("Users");
        }

        public string TakeUser(string login, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("login", login);

            var response = UserCollection.Find(filter).FirstOrDefault();

            if(response == null)
            {
                return "User not finded";
            }

            _user = BsonSerializer.Deserialize<User>(response);

            return _user.login;
        }

        public string NewUser(string login, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("login", login);

            var checkUser = UserCollection.Find(filter).FirstOrDefault();

            if(checkUser != null)
            {
                return "User already created";
            }

            var document = new BsonDocument { { "login", login }, {"password", password}};

            UserCollection.InsertOne(document);

            checkUser = UserCollection.Find(filter).FirstOrDefault();

            _user = BsonSerializer.Deserialize<User>(checkUser);

            return _user.login;
        }
    }
}