using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    public class MockMusicArtist : IMusicArtist
    {
        public MockMusicArtist(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
        public Uri ImageUri { get; }
    }
}
