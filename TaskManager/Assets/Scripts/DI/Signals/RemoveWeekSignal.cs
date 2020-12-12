namespace Assets.Scripts.DI.Signals
{
    public class RemoveWeekSignal
    {
        public WeekController Week { get; set; }

        public RemoveWeekSignal(WeekController week)
        {
            Week = week;
        }
    }
}
