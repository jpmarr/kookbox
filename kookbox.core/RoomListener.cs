using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    internal class RoomListener : IMusicRoomListener
    {
        private readonly IMusicRoom room;

        public RoomListener(IMusicRoom room, IMusicListener listener)
        {
            this.room = room;
            Listener = listener;
        }

        public IMusicListener Listener { get; }
        public bool IsConnected { get; }
        public IEnumerable<IMusicListenerRole> RoomRoles { get; }
        public Option<IPoll> Poll { get; }
        public Option<IBan> Ban { get; }
    }
}
