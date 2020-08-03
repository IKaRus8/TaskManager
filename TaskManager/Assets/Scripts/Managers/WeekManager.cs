using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeekManager : MonoBehaviour
{
    public static WeekManager Instance;

    private PanelManager _panelManager => PanelManager.Instance;

    public List<WeekController> Weeks { get; set; }
    public WeekController CurrentWeek { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Weeks = new List<WeekController>();
    }

    public void Add(List<WeekController> weeks)
    {
        weeks.OrderBy(w => w?.startWeek)?.ToList().ForEach(w => Add(w));

        SetCurrentWeek();
    }

    public void Add(WeekController newWeek)
    {
        if (newWeek != null)
        {
            newWeek.FillDays();

            Weeks.Add(newWeek);
        }
    }

    public void Add(string weekName)
    {
        WeekController newWeek = new WeekController(Weeks.Count + 1, weekName);

        if (Weeks.Any())
        {
            newWeek.startWeek = GetNextWeekStartDay(Weeks.Last().startWeek);
        }
        else
        {
            DateTime input = DateTime.Now;
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            newWeek.startWeek = input.AddDays(delta);
        }

        Weeks.Add(newWeek);

        StorageManager.Update(newWeek);
    }

    public void WeekDialogConstruct()
    {
        _panelManager.WeekDialogConstruct(Weeks.Count, Add);
    }

    public static DateTime GetNextWeekStartDay(DateTime start, DayOfWeek day = DayOfWeek.Monday)
    {
        start = start.AddDays(1);

        int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }

    public void SetCurrentWeek()
    {
        var week = Weeks.FirstOrDefault(w => w.weekNumber == Weeks.Where(w1 => w1.startWeek <= DateTime.Now).Max(w2 => w2.weekNumber));

        if (week != null && week.startWeek.AddDays(7) < DateTime.Now)
        {
            Weeks = Weeks.OrderBy(w => w.weekNumber).ToList();

            Weeks.FirstOrDefault().startWeek = GetNextWeekStartDay(week.startWeek);

            for (int i = 1; i < Weeks.Count; i++)
            {
                Weeks[i].startWeek = GetNextWeekStartDay(Weeks[i - 1].startWeek);
            }

            UpdateWeeks();

            //каждый раз проходит дальше
            SetCurrentWeek();
            return;
        }

        CurrentWeek = week;
    }

    private void UpdateWeeks()
    {
        Weeks.ForEach(w => w.UpdateWeek());
    }
}
