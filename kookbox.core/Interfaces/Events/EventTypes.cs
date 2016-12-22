using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces.Events
{
    public static class EventTypes
    {
        public static class Player
        {
            public const int StartPlaying = 101;
            public const int StopPlaying = 102;
            public const int PositionChange = 103;
            public const int PlaybackComplete = 104;

            public static Event CreateStartPlaying()
            {
                return Event.Create(StartPlaying);
            }

            public static Event CreateStopPlaying()
            {
                return Event.Create(StopPlaying);
            }

            public static Event CreatePositionChange()
            {
                return Event.Create(PositionChange);
            }

            public static Event CreatePlaybackComplete()
            {
                return Event.Create(PlaybackComplete);
            }
        }

        public static class Room
        {
            public const int CurrentTrackChanged = 201;
            public const int QueueChanged = 202;

            public static Event CreateCurrentTrackChanged(object data)
            {
                return Event.Create(CurrentTrackChanged, data);
            }

            public static Event CreateQueueChanged(IEnumerable<QueueChangeOperation> operations)
            {
                return Event.Create(QueueChanged, operations);
            }
        }
    }
}

