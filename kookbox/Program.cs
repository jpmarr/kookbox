using System;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core;
using kookbox.core.Interfaces;
using kookbox.mock;

namespace kookbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DoTheStuffAsync();

            Console.ReadLine();
        }

        private static async void DoTheStuffAsync()
        {
            var server = new Server();

            server.Sources.RegisterMusicSource(new MockMusicSource());

            var listener = await server.ConnectListenerAsync("jim");
            var room = await server.CreateRoomAsync(listener, "Test Room");

            var source = server.Sources.First();

            IMusicPlaylistSource playlist = null;
            var playlistFactory = source as IMusicPlaylistSourceFactory;
            if (playlistFactory != null)
                playlist = await playlistFactory.CreatePlaylistSourceAsync("random");

            room.DefaultTrackSource = Option.Some(playlist);

            await room.OpenAsync();
        }
    }
}
