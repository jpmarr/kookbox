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
        public IEnumerable<IMusicPlayerDescriptor> AllPlayers { get; }
        public IEnumerable<IMusicPlayerDescriptor> AvailablePlayers { get; }
        public Task<Option<IMusicPlayer>> RequestPlayerAsync(IMusicRoom room, string playerId)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IMusicPlayer>> RequestAvailablePlayerAsync(IMusicRoom room)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IMusicTrack>> GetTrackAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IMusicAlbum>> GetAlbumAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<IMusicArtist>> GetArtistAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IMusicSearchResults> SearchAsync(string searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
