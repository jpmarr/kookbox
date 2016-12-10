using System;
using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IAlbum
    {
        string Id { get; }
        string Name { get; }
        IArtist Artist { get; }
        IEnumerable<ITrack> Tracks { get; }
        Uri ImageUri { get; }
    }
}
