using System.Threading.Tasks;

namespace kookbox.core.Interfaces.Internal
{
    internal interface IRoomController : IRoom
    {
        IRoomUserController ConnectUser(IUserController user);
        void DisconnectUser(IRoomUser roomUser);

        Task OpenAsync();
        Task CloseAsync();
        Task PlayAsync();
        Task PauseAsync();
    }
}
