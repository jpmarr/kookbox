using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces.Internal
{
    internal interface IRoomController : IRoom
    {
        IRoomUser ConnectUser(IUser listener);
        void DisconnectUser(IRoomUser roomListener);

        Task OpenAsync();
        Task CloseAsync();
        Task PlayAsync();
        Task PauseAsync();
    }
}
