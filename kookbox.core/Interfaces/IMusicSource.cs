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
        IEnumerable<IPlayerDescriptor> AllPlayers { get; }
        IEnumerable<IPlayerDescriptor> AvailablePlayers { get; }
        Task<Option<IPlayer>> RequestPlayerAsync(IRoom room, string playerId);
        Task<Option<IPlayer>> RequestAvailablePlayerAsync(IRoom room);

        Task<Option<ITrack>> GetTrackAsync(string id);
        Task<Option<IAlbum>> GetAlbumAsync(string id);
        Task<Option<IArtist>> GetArtistAsync(string id);
        Task<ISearchResults> SearchAsync(string searchCriteria);
    }

    [Flags]
    public enum PlayerCapabilities
    {
        None,
        UnlimitedPlayback
    }
}
