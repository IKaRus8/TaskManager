namespace Assets.Scripts.DI.Signals
{
    public class EnableBackgroundSignal
    {
        public bool Value { get; set; }

        public EnableBackgroundSignal(bool value)
        {
            Value = value;
        }
    }
}