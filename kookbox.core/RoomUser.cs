using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Internal;

namespace kookbox.core
{
    internal class RoomUser : IRoomUserController
    {
        private readonly IRoomController roomController;
        private readonly IUserController userController;

        public RoomUser(IRoomController roomController, IUserController userController)
        {
            this.roomController = roomController;
            this.userController = userController;
        }

        public IRoom Room => roomController;
        public IUser User => userController;
        public bool IsConnected { get; }
        public IEnumerable<IUserRole> RoomRoles { get; }
        public Option<IPoll> Poll { get; }
        public Option<IBan> Ban { get; }

        public Task OpenRoomAsync()
        {
            return roomController.OpenAsync();
        }

        public Task CloseRooomAsync()
        {
            return roomController.CloseAsync();
        }

        public Task PlayRoomAsync()
        {
            return roomController.PlayAsync();
        }

        public Task PauseRoomAsync()
        {
            return roomController.PlayAsync();
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

        IRoomController IRoomUserController.RoomController => roomController;

        IUserController IRoomUserController.UserController => userController;
    }
}
