﻿using System;
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
        public IMusicPlayer Player { get; }

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
