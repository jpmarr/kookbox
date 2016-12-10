using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    /// <summary>
    /// Each server can host multiple 'rooms' that are created by a permitted user
    /// and have a particular playlist source for the ongoing music. A room may impose 
    /// specific restrictions on users, types of music allowed etc.
    /// </summary>
    public interface IRoom
    {
        string Id { get; }
        string Name { get; }
        IUser Creator { get; }
        Option<IPlaylistSource> DefaultTrackSource { get; set; }
        Option<IQueuedTrack> CurrentTrack { get; }
        ITrackQueue UpcomingQueue { get; }
        IEnumerable<IRoomUser> Users { get; }
        ISecurity Security { get; }

        IEnumerable<IQueuedTrack> GetTrackHistory(int count);
        RoomState State { get; }
    }

    public enum RoomState
    {
        Open,
        Playing,
        Closed
    }
}
