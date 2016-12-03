using System;
using System.Linq;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.DTO
{
    public class RoomInfo
    {
        public RoomInfo(string id, string name, ListenerInfo creator, QueuedTrackInfo currentTrack, PlaybackState playbackState,
            TimeSpan currentTrackPosition, int upcomingQueueLength, int listenerCount)
        {
            Id = id;
            Name = name;
            Creator = creator;
            CurrentTrack = currentTrack;
            PlaybackState = playbackState;
            CurrentTrackPosition = currentTrackPosition;
            UpcomingQueueLength = upcomingQueueLength;
            ListenerCount = listenerCount;
        }

        public static RoomInfo FromRoom(IMusicRoom room)
        {
            return new RoomInfo(
                room.Id, 
                room.Name, 
                ListenerInfo.FromListener(room.Creator), 
                QueuedTrackInfo.FromQueuedTrack(room.CurrentTrack.ValueOr(null)),
                PlaybackState.Playing, 
                TimeSpan.MinValue, 
                room.UpcomingQueue.Count,
                room.Listeners.Count());    
        }

        public string Id { get; }
        public string Name { get; }
        public ListenerInfo Creator { get; }
        public QueuedTrackInfo CurrentTrack { get; }
        public PlaybackState PlaybackState { get; }
        public TimeSpan CurrentTrackPosition { get; }
        public int UpcomingQueueLength { get; }
        public int ListenerCount { get; }
    }

    public enum PlaybackState
    {
        Idle,
        Paused,
        Playing,
        Stalled,
        Error
    }
}
