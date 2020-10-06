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

        //private static User CurrentUser;
        
        public static void Conect()
        {
            _client = new MongoClient("mongodb+srv://Rkarimov:12345@clustertask-nfnnn.gcp.mongodb.net/test?retryWrites=true&w=majority");
            _database = _client.GetDatabase("test");
            _userCollection = _database.GetCollection<User>("Users");
            _taskCollection = _database.GetCollection<TaskInfo>("Tasks");
        }

        public static User TakeUser(string login, string password)
        {
            User response = _userCollection.Find(u => u.login == login && u.password == password).FirstOrDefault();

            UserInfo.User = response;

            return response;
        }

        public static User NewUser(string login, string password)
        {
            User checkUser = _userCollection.Find(u => u.login == login).FirstOrDefault();

            if(checkUser != null)
            {
                UserInfo.User = checkUser;

                return checkUser;
            }

            checkUser = new User() { login = login, password = password, weeks = new List<WeekController>() };

            _userCollection.InsertOne(checkUser);

            //TODO: обработать ошибки
            checkUser = _userCollection.Find(u => u.login == login).FirstOrDefault();

            UserInfo.User = checkUser;

            return checkUser;
        }

        public static List<TaskInfo> GetTasksByUser()
        {
            List<TaskInfo> tasks = _taskCollection.Find(t => t._userId == UserInfo.User._id && !t.deleted)?.ToList();

            return tasks;
        }

        public static List<TaskInfo> GetTasksByWeek(string weekName)
        {
            List<TaskInfo> tasks = _taskCollection.Find(t => t._userId == UserInfo.User._id && t._weekName == weekName && !t.deleted)?.ToList();

            return tasks;
        }

        public static void AddTask(TaskInfo task)
        {
            task._userId = UserInfo.User._id;

            _taskCollection.InsertOneAsync(task);
        }

        public static void AddWeek(WeekController week)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, UserInfo.User._id);

            var update = Builders<User>.Update.AddToSet(u => u.weeks, week);

            var result = _userCollection.UpdateOneAsync(filter, update);
        }

        public static void UpdateTask(TaskInfo task)
        {
            var filter = Builders<TaskInfo>.Filter.Eq(t => t._id, task._id);

            //var update = Builders<TaskInfo>.Update.Set(t => t.deleted, task.deleted);

            var result = _taskCollection.ReplaceOneAsync(filter, task);
        }

        public static void RemoveWeek(WeekController week)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, UserInfo.User._id);

            var update = Builders<User>.Update.PullFilter(u => u.weeks, w => w.WeekName == week.WeekName);

            var result = _userCollection.FindOneAndUpdateAsync(u => u._id == UserInfo.User._id, update);
        }
    }
}