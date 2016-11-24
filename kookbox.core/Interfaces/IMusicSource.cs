using System.Collections;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    /// <summary>
    /// Sources of music will implement this (any other interfaces) eg Spotify Music Source, Sonos etc
    /// </summary>
    public interface IMusicSource
    {
        string Name { get; }
        IMusicPlayer Player { get; }

        Task<Option<IMusicTrack>> GetTrackAsync(string id);
        Task<Option<IMusicAlbum>> GetAlbumAsync(string id);
        Task<Option<IMusicArtist>> GetArtistAsync(string id);
        Task<IMusicSearchResults> SearchAsync(string searchCriteria);
    }
}
