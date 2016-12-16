using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.DTO
{
    public class UserInfo
    {
        public UserInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static UserInfo FromListener(IUser listener)
        {
            if (listener == null)
                return null;

            return new UserInfo(listener.Id, listener.Name);
        }

        public string Id { get; }
        public string Name { get; }
    }
}
