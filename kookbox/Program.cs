using System;
using System.Linq;
using kookbox.core;
using kookbox.core.Interfaces;
using kookbox.http;
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
            IMusicServer server = new Server();
            server.Sources.RegisterMusicSource(new MockMusicSource());
            server.Start();

            var http = new KookboxHttpServer(server);
            http.Start();


            /*
            var listener = await server.ConnectListenerAsync("jim", null);
            var room = await server.CreateRoomAsync(listener, "Test Room");

            var source = server.Sources.First();

            IMusicPlaylistSource playlist = null;
            var playlistFactory = source as IMusicPlaylistSourceFactory;
            if (playlistFactory != null)
                playlist = await playlistFactory.CreatePlaylistSourceAsync("random");

            room.DefaultTrackSource = Option.Some(playlist);

            await room.OpenAsync();
            */
        }
    }
}
