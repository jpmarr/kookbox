﻿using System;
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
        }
    }
}
