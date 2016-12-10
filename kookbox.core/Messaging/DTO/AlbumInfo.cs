using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.DTO
{
    public class AlbumInfo
    {
        public AlbumInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static AlbumInfo FromAlbum(IAlbum album)
        {
            if (album == null)
                return null;

            return new AlbumInfo(album.Id, album.Name);
        }

        public string Id { get; }
        public string Name { get; }
    }
}
