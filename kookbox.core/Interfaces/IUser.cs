using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IUser
    {
        string Id { get; }
        string Name { get; }
        bool IsConnected { get; }
        Option<IRoom> ActiveRoom { get; }
        Option<IBan> Ban { get; }
        IEnumerable<IUserRole> ServerRoles { get; }

        Task<IRoomUser> ConnectToRoomAsync(IRoom room);
        Task<IRoom> CreateRoomAsync(IUser creator, string name);

        Task StartListenerBanPollAsync(IUser listener);
        Task StartRoomSwitchPollAsync(IRoom newRoom);
        Task StartRoomBanPollAsync(IRoom room);

        Task VoteInPollAsync(IPoll poll);
    }
}
