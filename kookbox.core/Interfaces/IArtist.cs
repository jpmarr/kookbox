using System;

namespace kookbox.core.Interfaces
{
    public interface IArtist
    {
        string Id { get; }
        string Name { get; }
        Uri ImageUri { get; }
    }
}
