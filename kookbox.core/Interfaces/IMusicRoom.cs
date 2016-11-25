using System.Collections.Generic;
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
        string Name { get; }
        IMusicListener Creator { get; }
        IMusicPlaylistSource DefaultTrackSource { get; set; }
        Option<IQueuedMusicTrack> CurrentTrack { get; }
        IMusicQueue UpcomingQueue { get; }
        IEnumerable<IMusicRoomListener> Listeners { get; }

        IEnumerable<IQueuedMusicTrack> GetTrackHistory(int count);
        RoomState State { get; }

        Task OpenAsync();
        Task CloseAsync();

        Task PlayAsync();
        Task PauseAsync();
    }

    public enum RoomState
    {
        Open,
        Playing,
        Closed
    }
}
