using System;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IPlaylistSource
    {
        string Name { get; }
        PlaylistType PlaylistType { get; }
        bool IsExhausted { get; }

        Task<Option<ITrack>> GetNextTrackAsync();
    }

    [Flags]
    public enum PlaylistType
    {
        Unknown = 0,
        Random = 1,
        UserCurated = 2,
        DynamicGenre = 4,
        DynamicArtist = 8,
        DynamicTrack = 16,
        All = Random | UserCurated | DynamicGenre | DynamicArtist | DynamicTrack
    }
}
