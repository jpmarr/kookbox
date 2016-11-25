using System.Collections.Generic;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    internal class MusicListener : IMusicListener
    {
        private readonly IMusicServer server;

        public MusicListener(IMusicServer server, string name)
        {
            this.server = server;
            Name = name;
        }

        public string Name { get; }
        public bool IsConnected { get; }
        public Option<IMusicRoom> ActiveRoom { get; }
        public Option<IBan> Ban { get; }
        public IEnumerable<IMusicListenerRole> ServerRoles { get; }
    }
}
