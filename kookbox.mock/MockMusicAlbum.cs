using System;
using System.Collections.Generic;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    public class MockMusicAlbum : IMusicAlbum
    {
        private readonly List<IMusicTrack> tracks = new List<IMusicTrack>();

        public MockMusicAlbum(string id, string name, IMusicArtist artist, Uri imageUri)
        {
            Id = id;
            Name = name;
            Artist = artist;
            ImageUri = imageUri;

        }

        public string Id { get; }
        public string Name { get; }
        public IMusicArtist Artist { get; }
        public IEnumerable<IMusicTrack> Tracks => tracks;
        public Uri ImageUri { get; }

        internal void AddTrack(IMusicTrack track)
        {
            tracks.Add(track);
        }
    }
}
