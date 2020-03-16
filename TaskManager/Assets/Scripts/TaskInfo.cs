using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class TaskInfo
{
    [BsonIgnoreIfDefault]
    public ObjectId _userId;

    public ObjectId _id;

    public bool isRecurring;

    public string _name;

    public string _descriptionText;

    public bool _isDone;

    public bool deleted;

    public string _weekName;

    public DayOfWeek _dayOfWeek;

    public DateTime _date;
}
