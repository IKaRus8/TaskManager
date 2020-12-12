using Assets.Scripts.DI.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class WeekManager
{
    private UIManager _panelManager;

    public WeekController CurrentWeek { get; private set; }
    public List<WeekController> Weeks { get; private set; } = new List<WeekController>();

    [Inject]
    private void Construct(UIManager panelManager, SignalBus signalBus)
    {
        _panelManager = panelManager;

        signalBus.Subscribe<RemoveWeekSignal>(Remove);
    }

    public void Add(List<WeekController> weeks)
    {
        weeks.OrderBy(w => w?.startWeek)?.ToList().ForEach(w => Add(w));

        SetCurrentWeek();
    }

    private void Add(WeekController newWeek)
    {
        if (newWeek != null)
        {
            newWeek.FillDays();

            Weeks.Add(newWeek);
        }
    }

    public void Create(string weekName)
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

        SetCurrentWeek();
    }

    public void Remove(RemoveWeekSignal signal)
    {
        _ = Weeks.Remove(signal.Week);

        StorageManager.Remove(signal.Week);
    }

    public void WeekDialogConstruct()
    {
        var panel = _panelManager.CreatePanel<DialogWindowInput>(null);

        panel.input.text = TextStorage.Week + " " + Weeks.Count + 1;
        panel.ActionString = Create;
    }

    public static DateTime GetNextWeekStartDay(DateTime start, DayOfWeek day = DayOfWeek.Monday)
    {
        start = start.AddDays(1);

        int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }

    public void SetCurrentWeek()
    {
        var week = Weeks?.FirstOrDefault(w =>
        {
            var weeksBefore = Weeks?.Where(w1 => w1.startWeek <= DateTime.Now);

            if (weeksBefore != null && weeksBefore.Any())
            {
                var weekNumber = weeksBefore.Max(w2 => w2.weekNumber);

                return w.weekNumber == weekNumber;
            }

            return false;
        });
        
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