using System;

namespace kookbox.core.Interfaces
{
    public interface IBan
    {
        IMusicListener Initiator { get; }
        DateTimeOffset ExpiryTimestamp { get; }
        Option<string> Message { get; }
    }
}
