using DataBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserInfo
{
    public static User User { get; set; }

    public static List<TaskInfo> Tasks
    {
        get
        {
            List<TaskInfo> result = new List<TaskInfo>();

            User.weeks.ForEach(w =>
            {
                List<TaskInfo> temp = StorageManager.LoadTaskByWeek(w.WeekName);

                if(temp != null)
                {
                    result.AddRange(temp);
                }
            });

            var a = StorageManager.LoadTasksForUser();

            return a;
        }
    }
}
