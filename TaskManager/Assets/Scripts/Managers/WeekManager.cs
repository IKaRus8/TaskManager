using Assets.Scripts.DI.Signals;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class WeekManager
{
    private UIManager _panelManager;
    private SignalBus _signalBus;

    public WeekController CurrentWeek { get; private set; }
    public List<WeekController> Weeks { get; private set; } = new List<WeekController>();

    [Inject]
    private void Construct(UIManager panelManager, SignalBus signalBus)

    {
        _panelManager = panelManager;
        _signalBus = signalBus;
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

    public void Remove(string weekName)
    {
        WeekController week = Weeks.FirstOrDefault(w => w.WeekName == weekName);

        if (week != null)
        {
            Weeks.Remove(week);
        }

       // _signalBus.Fire(new SendMessageSignal(MessageTarget.Footer, TextStorage.RemoveLastWeekMessage));
    }

    public void WeekDialogConstruct()
    {
        var panel = _panelManager.CreatePanel<DialogWindowInput>(_panelManager.panelBack);

        if (panel != default)
        {
            panel.input.text = TextStorage.Week + " " + Weeks.Count + 1;
            panel.ActionString = Create;

            _signalBus.Fire(new SwitchOffPanelsSignal(null));
            _signalBus.Fire(new EnableBackgroundSignal(true));
        }
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