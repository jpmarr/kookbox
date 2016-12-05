﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    /// <summary>
    /// Each server can host multiple 'rooms' that are created by a permitted user
    /// and have a particular playlist source for the ongoing music. A room may impose 
    /// specific restrictions on users, types of music allowed etc.
    /// </summary>
    public interface IMusicRoom
    {
        string Id { get; }
        string Name { get; }
        IMusicListener Creator { get; }
        Option<IMusicPlaylistSource> DefaultTrackSource { get; set; }
        Option<IQueuedMusicTrack> CurrentTrack { get; }
        IMusicQueue UpcomingQueue { get; }
        IEnumerable<IMusicRoomListener> Listeners { get; }
        IMusicSecurity Security { get; }

        IEnumerable<IQueuedMusicTrack> GetTrackHistory(int count);
        RoomState State { get; }
    }

    public enum RoomState
    {
        Open,
        Playing,
        Closed
    }
}
