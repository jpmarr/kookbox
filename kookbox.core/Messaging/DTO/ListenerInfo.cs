using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.DTO
{
    public class ListenerInfo
    {
        public ListenerInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static ListenerInfo FromListener(IUser listener)
        {
            if (listener == null)
                return null;

            return new ListenerInfo(listener.Id, listener.Name);
        }

        public string Id { get; }
        public string Name { get; }
    }
}
