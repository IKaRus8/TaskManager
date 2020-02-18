using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DataBase
{
    public static class MongoDbAtlasManager
    {
        private static MongoClient _client;
        private static IMongoDatabase _database;
        private static IMongoCollection<User> _userCollection;
        private static IMongoCollection<TaskInfo> _taskCollection;

        private static User CurrentUser;
        
        public static void Conect()
        {
            _client = new MongoClient("mongodb+srv://Rkarimov:12345@clustertask-nfnnn.gcp.mongodb.net/test?retryWrites=true&w=majority");
            _database = _client.GetDatabase("test");
            _userCollection = _database.GetCollection<User>("Users");
            _taskCollection = _database.GetCollection<TaskInfo>("Tasks");
        }

        public static bool TakeUser(string login, string password)
        {
            User response = _userCollection.Find(u => u.login == login && u.password == password).FirstOrDefault();

            if(response == null)
            {
                return false;
            }

            CurrentUser = response;

            return true;
        }

        public static bool NewUser(string login, string password)
        {
            User checkUser = _userCollection.Find(u => u.login == login).FirstOrDefault();

            if(checkUser != null)
            {
                return false;
            }

            checkUser = new User() { login = login, password = password };

            _userCollection.InsertOne(checkUser);

            //TODO: обработать ошибки
            checkUser = _userCollection.Find(u => u.login == login).FirstOrDefault();

            CurrentUser = checkUser;

            return true;
        }

        public static List<TaskInfo> GetTasks()
        {
            var tasks = _taskCollection.Find(t => t._userId == CurrentUser._id)?.ToList();

            return tasks;
        }

        public static void AddTask(TaskInfo task)
        {
            task._userId = CurrentUser._id;

            _taskCollection.InsertOne(task);
        }
    }
}