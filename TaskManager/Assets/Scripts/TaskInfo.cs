using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class TaskInfo
{
    [BsonIgnoreIfDefault]
    public ObjectId _userId;

    public ObjectId _id;

    public bool isRecurring;

    public string _descriptionText;

    public bool _isDone;

    public bool deleted;

    public string _weekname;

    public DayOfWeek _dayOfWeek;
}
