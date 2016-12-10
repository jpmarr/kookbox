using System;
using System.Collections.Generic;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    public class MockMusicAlbum : IAlbum
    {
        private readonly List<ITrack> tracks = new List<ITrack>();

        public MockMusicAlbum(string id, string name, IArtist artist, Uri imageUri)
        {
            Id = id;
            Name = name;
            Artist = artist;
            ImageUri = imageUri;

        }

        public string Id { get; }
        public string Name { get; }
        public IArtist Artist { get; }
        public IEnumerable<ITrack> Tracks => tracks;
        public Uri ImageUri { get; }

        internal void AddTrack(ITrack track)
        {
            tracks.Add(track);
        }
    }
}
