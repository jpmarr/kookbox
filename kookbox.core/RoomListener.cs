using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Internal;

namespace kookbox.core
{
    internal class RoomListener : IMusicRoomListener
    {
        private readonly IMusicRoomController controller;

        public RoomListener(IMusicRoomController room, IMusicListener listener)
        {
            this.controller = room;
            Listener = listener;
        }

        public IMusicRoom Room => controller.Room;
        public IMusicListener Listener { get; }
        public bool IsConnected { get; }
        public IEnumerable<IMusicListenerRole> RoomRoles { get; }
        public Option<IPoll> Poll { get; }
        public Option<IBan> Ban { get; }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task OpenRoomAsync()
        {
            return controller.OpenAsync();
        }

        public Task CloseRooomAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteRoomAsync()
        {
            throw new NotImplementedException();
        }

        public Task PlayRoomAsync()
        {
            throw new NotImplementedException();
        }

        public Task PauseRoomAsync()
        {
            throw new NotImplementedException();
        }

        public Task RequestTrackAsync(IMusicTrack track, Option<IMusicDedication> dedication)
        {
            throw new NotImplementedException();
        }

        public Task StartTrackSkipPollAsync(IQueuedMusicTrack track)
        {
            throw new NotImplementedException();
        }

        public Task StartListenerBanPollAsync(IMusicRoomListener listener)
        {
            throw new NotImplementedException();
        }

        public Task VoteInPollAsync(IPoll poll, VoteType voteType)
        {
            throw new NotImplementedException();
        }
    }
}
