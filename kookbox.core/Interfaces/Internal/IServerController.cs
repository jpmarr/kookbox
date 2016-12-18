using System.Threading.Tasks;

namespace kookbox.core.Interfaces.Internal
{
    internal interface IServerController : IServer
    {
        Task<IRoomController> CreateRoomAsync(IUser creator, string name);
        IRoomController GetRoom(string id);
    }
}
