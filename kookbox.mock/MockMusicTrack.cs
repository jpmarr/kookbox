using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    internal class MockMusicTrack : IMusicTrack
    {
        public IMusicSource Source { get; }
        public string Title { get; }
        public int Number { get; }
        public IMusicArtist Artist { get; }
        public IMusicAlbum Album { get; }
        public TimeSpan Duration { get; }
        public IMusicListener Introducer { get; }
        public Option<IBan> Ban { get; }
    }
}
