using System;
using System.Linq;
using Unity.Notifications.Android;

public class NotificationManager
{
    public NotificationManager()
    {
        var chanels = AndroidNotificationCenter.GetNotificationChannels();

        if (!chanels.Any())
        {
            var chanel = new AndroidNotificationChannel()
            {
                Id = TextStorage.NotificationChanel_id,
                Name = TextStorage.NotificationChanel_name,
                Importance = Importance.Default,
                Description = TextStorage.NotificationChanel_Description,
            };
            AndroidNotificationCenter.RegisterNotificationChannel(chanel); 
        }

        ShowNotification("test", "fiveSec", DateTime.Now.Add(new TimeSpan(0,0,5)));
    }

    public void ShowNotification(string title, string text, DateTime dateTime)
    {
        var notification = new AndroidNotification
        {
            Title = title,
            Text = text,
            FireTime = dateTime
            //SmallIcon = "my_custom_icon_id"
            //LargeIcon = "my_custom_large_icon_id"
        };

        var a = AndroidNotificationCenter.SendNotification(notification, TextStorage.NotificationChanel_id);
    }
}
