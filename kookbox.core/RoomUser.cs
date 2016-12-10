using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Internal;

namespace kookbox.core
{
    internal class RoomUser : IRoomUser
    {
        private readonly IRoomController controller;

        public RoomUser(IRoomController room, IUser listener)
        {
            this.controller = room;
            Listener = listener;
        }

        public IRoom Room => controller;
        public IUser Listener { get; }
        public bool IsConnected { get; }
        public IEnumerable<IUserRole> RoomRoles { get; }
        public Option<IPoll> Poll { get; }
        public Option<IBan> Ban { get; }

        public Task OpenRoomAsync()
        {
            return controller.OpenAsync();
        }

        public Task CloseRooomAsync()
        {
            return controller.CloseAsync();
        }

        public Task PlayRoomAsync()
        {
            return controller.PlayAsync();
        }

        public Task PauseRoomAsync()
        {
            return controller.PlayAsync();
        }

        public Task RequestTrackAsync(ITrack track, Option<IDedication> dedication)
        {
            throw new NotImplementedException();
        }

        public Task StartTrackSkipPollAsync(IQueuedTrack track)
        {
            throw new NotImplementedException();
        }

        public Task StartListenerBanPollAsync(IRoomUser listener)
        {
            throw new NotImplementedException();
        }

        public Task VoteInPollAsync(IPoll poll, VoteType voteType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRoomAsync()
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }
    }
}
