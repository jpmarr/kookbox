using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox.mock
{
    public class MockMusicSource : 
        IMusicSource, 
        IPlaylistSourceFactory
    {
        private readonly Dictionary<string, MockMusicPlayer> players = new Dictionary<string, MockMusicPlayer>();
        private List<IArtist> artists;
        private List<ITrack> tracks = new List<ITrack>();
        private List<IAlbum> albums = new List<IAlbum>();
        private ITrack[] randomTracks;
        private int randomTrackIndex;

        public MockMusicSource()
        {
            players.Add("player1", new MockMusicPlayer(this, "player1", "The first player"));
            players.Add("player2", new MockMusicPlayer(this, "player2", "The second player"));
            CreateFakeMusicLibrary();
        }

        public string Name => "Mock";
        public PlayerCapabilities PlayerCapabilities => PlayerCapabilities.None;
        public IEnumerable<IPlayerDescriptor> AllPlayers => players.Values;
        public IEnumerable<IPlayerDescriptor> AvailablePlayers => players.Values.Where(p => !p.CurrentRoom.HasValue);
        public PlaylistType SupportedPlaylistTypes => PlaylistType.All;

        public Task<Option<IPlayer>> RequestPlayerAsync(IRoom room, string playerId)
        {
            MockMusicPlayer player;
            if (players.TryGetValue(playerId, out player))
            {
                if (player.CurrentRoom.HasValue)
                {
                    // todo: protocol to request player transfer if room is open - other wise if room is closed we just take it?
                }
                player.AssignToRoom(room);
            }

            return Task.FromResult(Option.Create((IPlayer)player));
        }

        public Task<Option<IPlayer>> RequestAvailablePlayerAsync(IRoom room)
        {
            var player = players.Values.FirstOrDefault(p => !p.CurrentRoom.HasValue);
            player?.AssignToRoom(room);

            return Task.FromResult(Option.Create((IPlayer)player));
        }

        public Task<Option<ITrack>> GetTrackAsync(string id)
        {
            return Task.FromResult(Option.Create(tracks.FirstOrDefault(t => t.Id == id)));
        }

        public Task<Option<IAlbum>> GetAlbumAsync(string id)
        {
            return Task.FromResult(Option.Create(albums.FirstOrDefault(t => t.Id == id)));
        }

        public Task<Option<IArtist>> GetArtistAsync(string id)
        {
            return Task.FromResult(Option.Create(artists.FirstOrDefault(t => t.Id == id)));
        }

        public Task<ISearchResults> SearchAsync(string searchCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<IPlaylistSource> CreatePlaylistSourceAsync(string configuration)
        {
            return Task.FromResult<IPlaylistSource>(new MockMusicPlaylistSource(this));
        }

        internal ITrack GetNextRandomTrack()
        {
            return randomTracks[randomTrackIndex++%randomTracks.Length];
        }

        private void CreateFakeMusicLibrary()
        {
            artists = (
                from i in Enumerable.Range(1, 100)
                select new MockMusicArtist(
                    Guid.NewGuid().ToString(), "Artist " + i) as IArtist
            ).ToList();

            var rng = new Random();

            albums = (
                from artist in artists
                from albumNumber in Enumerable.Range(1, rng.Next(1, 7))
                let id = Guid.NewGuid().ToString()
                select new MockMusicAlbum(
                    id, $"{artist.Name} - Album {albumNumber}", artist, new Uri("http://albumart/" + id)) as IAlbum
            ).ToList();

            tracks = (
                from album in albums
                from trackNumber in Enumerable.Range(1, rng.Next(5, 25))
                select
                    new MockMusicTrack(this, Guid.NewGuid().ToString(), $"{album.Artist.Name} - Album Track {trackNumber}", album.Artist,
                        GetRandomTrackDuration(rng)) as ITrack).ToList();

            // some non-album tracks to mix it up
            tracks.AddRange(
                from artist in artists
                from trackNumber in Enumerable.Range(1, rng.Next(0, 50))
                select
                new MockMusicTrack(this, Guid.NewGuid().ToString(), $"{artist.Name} - Track {trackNumber}", artist,
                    GetRandomTrackDuration(rng)));

            randomTracks = tracks.OrderBy(t => Guid.NewGuid()).ToArray();
        }

        private TimeSpan GetRandomTrackDuration(Random rng)
        {
            return TimeSpan.FromMilliseconds(rng.Next(2000, 4000));
            //return TimeSpan.FromMilliseconds(rng.Next(120000, 300000));
        }
    }
}
