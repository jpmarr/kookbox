namespace kookbox.core.Interfaces
{
    public interface IMusicPlaylistSource
    {
        string Name { get; }
        IMusicTrack GetNextTrack();
    }
}
