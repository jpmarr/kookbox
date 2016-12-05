using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicRoomListener
    {
        IMusicRoom Room { get; }
        IMusicListener Listener { get; }
        bool IsConnected { get; }
        IEnumerable<IMusicListenerRole> RoomRoles { get; }
        Option<IPoll> Poll { get; }
        Option<IBan> Ban { get; }

        Task DisconnectAsync(); // from room

        Task OpenRoomAsync();
        Task CloseRooomAsync();
        Task DeleteRoomAsync();

        Task PlayRoomAsync();
        Task PauseRoomAsync();

        Task RequestTrackAsync(IMusicTrack track, Option<IMusicDedication> dedication);
        Task StartTrackSkipPollAsync(IQueuedMusicTrack track);
        Task StartListenerBanPollAsync(IMusicRoomListener listener);

        Task VoteInPollAsync(IPoll poll, VoteType voteType);
    }
}
