using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    internal class MockMusicTrack : ITrack
    {
        public MockMusicTrack(IMusicSource source, string id, string title, IArtist artist, TimeSpan duration)
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
            Album = Option.Some(album as IAlbum);
            album.AddTrack(this);
        }

        public string Id { get; }
        public IMusicSource Source { get; }
        public string Title { get; }
        public int Number { get; }
        public IArtist Artist { get; }
        public Option<IAlbum> Album { get; }
        public TimeSpan Duration { get; }
        public IUser Introducer { get; }
        public Option<IBan> Ban { get; }
        public Uri ImageUri { get; }
    }
}
