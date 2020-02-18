using MongoDB.Bson;
using System.Collections.Generic;

namespace DataBase
{
    public class User
    {
        //[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        //[BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id;

        public string login;
        public string password;
        public List<WeekController> weeks;
    }
}
