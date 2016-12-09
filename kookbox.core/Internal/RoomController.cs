using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Interfaces.Internal;

namespace kookbox.core.Internal
{
    public class RoomController : IMusicRoomController
    {
        public RoomController(IMusicRoom room)
        {
            Room = room;
        }

        public IMusicRoom Room { get; }

        public Task OpenAsync()
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync()
        {
            throw new NotImplementedException();
        }
    }
}
