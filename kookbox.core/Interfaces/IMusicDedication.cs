namespace kookbox.core.Interfaces
{
    public interface IMusicDedication
    {
        IMusicListener DedicatedTo { get; }
        Option<string> Message { get; }
    }
}
