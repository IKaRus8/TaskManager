namespace Assets.Scripts.DI.Signals
{
    public class SwitchOffPanelsSignal
    {
        public BasePanel Panel { get; set; }

        public SwitchOffPanelsSignal(BasePanel panel)
        {
            Panel = panel;
        }
    }
}
