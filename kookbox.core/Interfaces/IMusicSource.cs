using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    /// <summary>
    /// Sources of music will implement this (any other interfaces) eg Spotify Music Source, Sonos etc
    /// </summary>
    public interface IMusicSource
    {
        string Name { get; }

        PlayerCapabilities PlayerCapabilities { get; }

        // A source may expose multiple players (ie Sonos each room is a player, Spotify - each registered account will have a player)
        // Rooms must request a player since each player can only support one room at a a time
        // If a source declares UnlimitedPlayback capabiltiies then PlayersAvailable will throw NotSupported and any RequestPlayer call will succeed
        IEnumerable<IMusicPlayerDescriptor> AllPlayers { get; }
        IEnumerable<IMusicPlayerDescriptor> AvailablePlayers { get; }
        Task<Option<IMusicPlayer>> RequestPlayerAsync(IMusicRoom room, string playerId);
        Task<Option<IMusicPlayer>> RequestAvailablePlayerAsync(IMusicRoom room);

        Task<Option<IMusicTrack>> GetTrackAsync(string id);
        Task<Option<IMusicAlbum>> GetAlbumAsync(string id);
        Task<Option<IMusicArtist>> GetArtistAsync(string id);
        Task<IMusicSearchResults> SearchAsync(string searchCriteria);
    }

    [Flags]
    public enum PlayerCapabilities
    {
        None,
        UnlimitedPlayback
    }
}
