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
        public MockMusicTrack(IMusicSource source, string id, string title, IMusicArtist artist, TimeSpan duration)
        {
            Id = id;
            Source = source;
            Title = title;
            Artist = artist;
            Duration = duration;
        }

        public MockMusicTrack(IMusicSource source, string id, string title, MockMusicAlbum album, TimeSpan duration) 
            : this(source, id, title, album.Artist, duration)
        {
            Album = Option.Some(album as IMusicAlbum);
            album.AddTrack(this);
        }

        public string Id { get; }
        public IMusicSource Source { get; }
        public string Title { get; }
        public int Number { get; }
        public IMusicArtist Artist { get; }
        public Option<IMusicAlbum> Album { get; }
        public TimeSpan Duration { get; }
        public IMusicListener Introducer { get; }
        public Option<IBan> Ban { get; }
        public Uri ImageUri { get; }
    }
}
