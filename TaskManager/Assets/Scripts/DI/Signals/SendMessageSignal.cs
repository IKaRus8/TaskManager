using Assets.Scripts.Enums;

namespace Assets.Scripts.DI.Signals
{
    public class SendMessageSignal
    {
        public MessageTarget Target { get; set; }
        public string Message { get; set; }

        public SendMessageSignal(MessageTarget target, string message)
        {
            Target = target;

            Message = message;
        }
    }
}
