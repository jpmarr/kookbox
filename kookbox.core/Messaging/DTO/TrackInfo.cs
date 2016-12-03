using System;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.DTO
{
    public class TrackInfo
    {
        public TrackInfo(string id, string title, TimeSpan duration, AlbumInfo album, ArtistInfo artist)
        {
            Id = id;
            Title = title;
            Duration = duration;
            Album = album;
            Artist = artist;
        }

        public static TrackInfo FromTrack(IMusicTrack track)
        {
            return new TrackInfo(
                track.Id, 
                track.Title, 
                track.Duration,
                AlbumInfo.FromAlbum(track.Album.ValueOr(null)),
                ArtistInfo.FromArtist(track.Artist));
        }

        public string Id { get; }
        public string Title { get; }
        public TimeSpan Duration { get; }
        public AlbumInfo Album { get; }
        public ArtistInfo Artist { get; }
    }
}
