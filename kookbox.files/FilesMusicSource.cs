using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox.files
{
    public class FilesMusicSource : IMusicSource
    {
        public FilesMusicSource()
        {
        }

        public string Name => "Files";
        public PlayerCapabilities PlayerCapabilities { get; }
        public IEnumerable<IPlayerDescriptor> AllPlayers { get; }
        public IEnumerable<IPlayerDescriptor> AvailablePlayers { get; }
        public Task<Option<IPlayer>> RequestPlayerAsync(IRoom room, string playerId)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IPlayer>> RequestAvailablePlayerAsync(IRoom room)
        {
            throw new NotImplementedException();
        }

        public Task<Option<ITrack>> GetTrackAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IAlbum>> GetAlbumAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IArtist>> GetArtistAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResults> SearchAsync(string searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
