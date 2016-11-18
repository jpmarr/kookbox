namespace kookbox.core.Interfaces
{
    /// <summary>
    /// Sources of music will implement this (any other interfaces) eg Spotify Music Source, Sonos etc
    /// </summary>
    public interface IMusicSource
    {
        string Name { get; }
        IMusicPlayer Player { get; }
    }
}
