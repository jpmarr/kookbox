using System;

namespace kookbox.core.Interfaces
{
    public interface IMusicArtist
    {
        string Id { get; }
        string Name { get; }
        Uri ImageUri { get; }
    }
}
