﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public static User TakeUser(string login, string password)
        {
            User response = _userCollection.Find(u => u.login == login && u.password == password).FirstOrDefault();

            CurrentUser = response;

            return CurrentUser;
        }

        public static User NewUser(string login, string password)
        {
            User checkUser = _userCollection.Find(u => u.login == login).FirstOrDefault();

            if(checkUser != null)
            {
                CurrentUser = checkUser;

                return CurrentUser;
            }

            checkUser = new User() { login = login, password = password, weeks = new List<WeekController>() };

            _userCollection.InsertOne(checkUser);

            //TODO: обработать ошибки
            checkUser = _userCollection.Find(u => u.login == login).FirstOrDefault();

            CurrentUser = checkUser;

            return CurrentUser;
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

        public static void UpdateUser(WeekController week)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, CurrentUser._id);

            var update = Builders<User>.Update.AddToSet(u => u.weeks, week);

            var result = _userCollection.UpdateOne(filter, update);
        }
    }
}