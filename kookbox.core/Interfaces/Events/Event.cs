using System.Dynamic;

namespace kookbox.core.Interfaces.Events
{
    public class Event
    {
        // todo: pooling?
        public static Event Create(int eventType, object data)
        {
            return new Event(eventType, data);
        }

        public static Event Create(int eventType)
        {
            return new Event(eventType, null);
        }

        private Event(int eventType, object data)
        {
            EventType = eventType;
            Data = Option.Create(data);
        }

        public int EventType { get; }
        public Option<object> Data { get; }
    }
}
