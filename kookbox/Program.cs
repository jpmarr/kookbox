using System.Linq;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;

namespace kookbox
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var server = new Server();
            var listener = await server.ConnectListenerAsync("jim");
            var room = await server.CreateRoomAsync(listener, "Test Room");

            var source = server.SourceSet.AllSources.First();

            IMusicPlaylistSource playlist = null;
            var playlistFactory = source as IMusicPlaylistSourceFactory;
            if (playlistFactory != null)
                playlist = await playlistFactory.CreatePlaylistSourceAsync("random");

            room.DefaultTrackSource = playlist;

            await room.OpenAsync();
        }
    }
}
