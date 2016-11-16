using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    /// <summary>
    /// Sources of music will implement this (any other interfaces) eg Spotify Music Source, Sonos etc
    /// </summary>
    public interface IMusicSource
    {
        string Name { get; }
    }
}
