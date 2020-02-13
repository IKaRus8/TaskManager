using MongoDB.Bson;

namespace DataBase
{
    public class User
    {
        //[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        //[BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id;

        public string login;
        public string password;

    }
}
