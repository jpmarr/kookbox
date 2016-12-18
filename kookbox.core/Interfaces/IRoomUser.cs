using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IRoomUser
    {
        IRoom Room { get; }
        IUser User { get; }
        IEnumerable<IUserRole> RoomRoles { get; }
        Option<IPoll> Poll { get; }
        Option<IBan> Ban { get; }

        Task DisconnectAsync(); // from room

        Task OpenRoomAsync();
        Task CloseRooomAsync();
        Task DeleteRoomAsync();

        Task PlayRoomAsync();
        Task PauseRoomAsync();

        Task RequestTrackAsync(ITrack track, Option<IDedication> dedication);
        Task StartTrackSkipPollAsync(IQueuedTrack track);
        Task StartListenerBanPollAsync(IRoomUser listener);

        Task VoteInPollAsync(IPoll poll, VoteType voteType);
    }
}
