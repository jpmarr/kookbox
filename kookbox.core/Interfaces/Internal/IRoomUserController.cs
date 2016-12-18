namespace kookbox.core.Interfaces.Internal
{
    internal interface IRoomUserController : IRoomUser
    {
        IRoomController RoomController { get; }
        IUserController UserController { get; }
    }
}
