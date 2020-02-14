using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Linq;

namespace DataBase
{
    public class MongoDbAtlasManager
    {
        public MongoClient _client;
        public IMongoDatabase _database;
        public IMongoCollection<BsonDocument> _userCollection;
        public IMongoCollection<BsonDocument> _taskCollection;

        private User _user;

        public void Conect()
        {
            _client = new MongoClient("mongodb+srv://Rkarimov:12345@clustertask-nfnnn.gcp.mongodb.net/test?retryWrites=true&w=majority");
            _database = _client.GetDatabase("test");
            _userCollection = _database.GetCollection<BsonDocument>("Users");
        }

        public string TakeUser(string login, string password)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("login", login);

            var response = _userCollection.Find(filter).FirstOrDefault();

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

            var checkUser = _userCollection.Find(filter).FirstOrDefault();

            if(checkUser != null)
            {
                return "User already created";
            }

            var document = new BsonDocument { { "login", login }, {"password", password}};

            _userCollection.InsertOne(document);

            checkUser = _userCollection.Find(filter).FirstOrDefault();

            _user = BsonSerializer.Deserialize<User>(checkUser);

            return _user.login;
        }
    }
}