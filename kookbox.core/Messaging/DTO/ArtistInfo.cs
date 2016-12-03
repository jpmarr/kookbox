using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.DTO
{
    public class ArtistInfo
    {
        public ArtistInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static ArtistInfo FromArtist(IMusicArtist artist)
        {
            if (artist == null)
                return null;

            return new ArtistInfo(artist.Id, artist.Name);
        }

        public string Id { get; }
        public string Name { get; }
    }
}
