namespace kookbox.core.Interfaces
{
    public interface IDedication
    {
        IUser DedicatedTo { get; }
        Option<string> Message { get; }
    }
}
